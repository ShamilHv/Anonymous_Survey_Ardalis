using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.UseCases.Admins.Queries.Get;
using Anonymous_Survey_Ardalis.UseCases.Subjects.Queries;
using Anonymous_Survey_Ardalis.Web.Admins;
using Anonymous_Survey_Ardalis.Web.Admins.Auth.Login;
using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Anonymous_Survey_Ardalis.Web.Security;

public class AuthService(
  IHttpContextAccessor httpContextAccessor,
  IRepository<Admin> repository,
  IMediator mediator,
  IPasswordHasher<Admin> passwordHasher,
  IConfiguration configuration) : IAuthService
{
  public async Task<Admin?> RegisterAsync(AuthRequest request)
  {
    // Check if email exists and subject is valid
    var result = await mediator.Send(new GetAdminByEmailQuery(request.Email));
    var subjectResult = await mediator.Send(new GetSubjectQuery(request.SubjectId));

    if (!result.IsNotFound())
    {
      throw new Exception("Admin with this email already exists");
    }

    if (subjectResult is null || !subjectResult.IsSuccess)
    {
      throw new Exception($"Subject id: {request.SubjectId} is invalid");
    }

    var newAdmin = new Admin(request.AdminName, request.Email, request.SubjectId);
    var passwordHash = passwordHasher.HashPassword(newAdmin, request.Password);
    newAdmin.PasswordHash = passwordHash!;
    newAdmin.CreatedAt = DateTime.UtcNow;
    var addedAdmin = await repository.AddAsync(newAdmin);
    await repository.SaveChangesAsync(); // Make sure this is not commented out!

    return addedAdmin;
  }
  // public async Task<Admin?> RegisterAsync(AuthRequest request)
  // {
  //   var result = await mediator.Send(new GetAdminByEmailQuery(request.Email));
  //   var subjectId = await mediator.Send(new GetSubjectQuery(request.SubjectId));
  //   if (!result.IsNotFound())
  //   {
  //     throw new Exception("Admin wtih this email already exists");
  //   }
  //
  //   if (subjectId is null)
  //   {
  //     throw new Exception($"Subject id: {subjectId} is invalid");
  //   }
  //
  //   var newAdmin = new Admin(request.AdminName, request.Email, request.SubjectId);
  //   var passwordHash = passwordHasher.HashPassword(newAdmin, request.Password);
  //   Console.WriteLine($"Generated hash length: {passwordHash?.Length}, hash: {passwordHash}");
  //
  //   newAdmin.PasswordHash = passwordHash!;
  //   Console.WriteLine($"Before add - Hash length: {newAdmin.PasswordHash?.Length}, hash: {newAdmin.PasswordHash}");
  //
  //   var addedAdmin = await repository.AddAsync(newAdmin);
  //
  //   if (string.IsNullOrEmpty(addedAdmin.PasswordHash))
  //   {
  //     Console.WriteLine("WARNING: Password hash is empty after adding to repository!");
  //   }
  //   else
  //   {
  //     Console.WriteLine($"After add - Hash length: {addedAdmin.PasswordHash.Length}");
  //   }
  //
  //   //await repository.SaveChangesAsync();
  //
  //   var verifiedAdmin = await repository.GetByIdAsync(addedAdmin.Id);
  //   if (verifiedAdmin != null)
  //   {
  //     Console.WriteLine($"After save - Hash in DB length: {verifiedAdmin.PasswordHash?.Length}, hash: {verifiedAdmin.PasswordHash}");
  //   }
  //   else
  //   {
  //     Console.WriteLine("Could not verify admin after saving");
  //   }
  //
  //   return addedAdmin;
  // }


  public async Task<AdminRecord> GetCurrentAdmin()
  {
    var adminId = GetCurrentAdminId();
    var result = await mediator.Send(new GetAdminQuery(adminId));
    if (result.Status != ResultStatus.Ok || result.Value == null)
    {
      throw new Exception("Admin could not be found");
    }

    return new AdminRecord(result.Value.Id, result.Value.AdminName, result.Value.Email, result.Value.SubjectId,
      result.Value.CreatedAt);
  }


  public async Task<AuthResponse?> LoginRequestAsync(LoginRequest loginRequest)
  {
    var result = await mediator.Send(new GetAdminByEmailQuery(loginRequest.Email));
    if (result.Status != ResultStatus.Ok || result.Value == null)
    {
      throw new Exception("Admin with this email does not exist");
    }

    var admin = result.Value;
    var passwordVerificationResult =
      passwordHasher.VerifyHashedPassword(admin, admin.PasswordHash, loginRequest.Password);

    if (passwordVerificationResult == PasswordVerificationResult.Failed)
    {
      throw new Exception("Invalid password");
    }

    var refreshToken = GenerateRefreshToken();
    result.Value.RefreshToken = refreshToken;
    await repository.SaveChangesAsync();
    var response = new AuthResponse
    {
      Admin = new AdminRecord(admin.Id, admin.AdminName, admin.Email, admin.SubjectId, admin.CreatedAt),
      Token = CreateToken(admin),
      RefreshToken = refreshToken,
      RefreshTokenExpiryTime = null
    };
    return response;
  }

  public async Task<TokenResponse?> RefreshTokensAsync(TokenRequest request)
  {
    var admin = await ValidateRefreshTokenAsync(request.AdminId, request.RefreshToken);
    if (admin is null)
    {
      return null;
    }

    return await CreateTokenResponse(admin);
  }

  private async Task<TokenResponse> CreateTokenResponse(Admin admin)
  {
    return new TokenResponse
    {
      Token = CreateToken(admin),
      RefreshToken = await GenerateAndSaveRefreshTokenAsync(admin),
      RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(5)
    };
  }

  private async Task<Admin> ValidateRefreshTokenAsync(int adminId, string refreshToken)
  {
    var admin = await mediator.Send(new GetAdminQuery(adminId));
    if (admin is null || admin.Value.RefreshToken != refreshToken ||
        admin.Value.RefreshTokenExpiryTime <= DateTime.UtcNow)
    {
      throw new Exception("Could not validate refresh token");
    }

    return admin;
  }

  private string GenerateRefreshToken()
  {
    var randomNumber = new byte[32];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    return Convert.ToBase64String(randomNumber);
  }

  private async Task<string> GenerateAndSaveRefreshTokenAsync(Admin admin)
  {
    var refreshToken = GenerateRefreshToken();
    admin.RefreshToken = refreshToken;
    admin.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(2);
    await repository.SaveChangesAsync();
    return refreshToken;
  }

  private string CreateToken(Admin admin)
  {
    var claims = new List<Claim>
    {
      new(ClaimTypes.Name, admin.AdminName), new(ClaimTypes.NameIdentifier, admin.Id.ToString())
    };
    var key = new SymmetricSecurityKey(
      Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
    var tokenDescriptor = new JwtSecurityToken(
      configuration.GetValue<string>("AppSettings:Issuer"),
      configuration.GetValue<string>("AppSettings:Audience"),
      claims,
      expires: DateTime.UtcNow.AddHours(8),
      signingCredentials: creds
    );
    return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
  }

  private string GetCurrentAdminName()
  {
    var httpContext = httpContextAccessor.HttpContext;
    if (httpContext == null)
    {
      throw new InvalidOperationException("HttpContext is not available");
    }

    var adminName = httpContext.User.FindFirstValue(ClaimTypes.Name);

    if (string.IsNullOrEmpty(adminName))
    {
      throw new Exception("Admin name claim not found in context");
    }

    return adminName;
  }

  private int GetCurrentAdminId()
  {
    var httpContext = httpContextAccessor.HttpContext;
    if (httpContext == null)
    {
      throw new InvalidOperationException("HttpContext is not available");
    }

    var adminIdString = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

    if (string.IsNullOrEmpty(adminIdString))
    {
      throw new Exception("Admin ID claim not found in context");
    }

    if (!int.TryParse(adminIdString, out var adminId))
    {
      throw new Exception("Admin ID is not in valid format");
    }

    return adminId;
  }
}
