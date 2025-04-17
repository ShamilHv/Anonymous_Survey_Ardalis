using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.UseCases.Admins;
using Anonymous_Survey_Ardalis.UseCases.Admins.Queries.Get;
using Anonymous_Survey_Ardalis.UseCases.Subjects.Queries;
using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Anonymous_Survey_Ardalis.Web.Security;

public class AuthService(IRepository<Admin> repository, IMediator mediator, IPasswordHasher<Admin> passwordHasher, IConfiguration configuration)  : IAuthService
{
  public async Task<Admin?> RegisterAsync(AuthenticationRequest authenticationRequest)
  {
    var admin = await mediator.Send(new GetAdminByEmailQuery(authenticationRequest.Email));  
    var subjectId=await mediator.Send(new GetSubjectQuery(authenticationRequest.SubjectId));
    if (admin != null)
    {
      throw new Exception("Admin wtih this email already exists");
    }
    if (subjectId is null)
    {
      throw new Exception($"Subject id: {subjectId} is invalid");
    }
    var newAdmin = new Admin(authenticationRequest.AdminName, authenticationRequest.Email, authenticationRequest.SubjectId);
    string passwordHash = new PasswordHasher<Admin>().HashPassword(newAdmin, authenticationRequest.Password);
    
    newAdmin.PasswordHash = passwordHash;
    newAdmin.CreatedAt = DateTime.UtcNow;

    await repository.AddAsync(newAdmin);
    await repository.SaveChangesAsync();
    return newAdmin;
  }

  public async Task<AuthenticationResponse?> LoginRequestAsync(LoginRequest loginRequest)
  {
    var result = await mediator.Send(new GetAdminByEmailQuery(loginRequest.Email));   
    if (result.Status != ResultStatus.Ok || result.Value == null)
    {
      throw new Exception("Invalid login request");
    }
    var admin = result.Value;
    var passwordVerificationResult = passwordHasher.VerifyHashedPassword(admin, admin.PasswordHash, loginRequest.Password);

    if (passwordVerificationResult == PasswordVerificationResult.Failed)
    {
     throw new Exception("Invalid password");
    }
    var response = new AuthenticationResponse
    {
      Admin=new AdminDto(admin.Id, admin.AdminName, admin.Email, admin.SubjectId,admin.CreatedAt),
      Token = CreateToken(admin), 
      RefreshToken = GenerateRefreshToken(),
      RefreshTokenExpiryTime = null
    };
    return response;
  }

  public async Task<TokenResponse?> RefreshTokensAsync(TokenRequest request)
  {
    Admin? admin = await ValidateRefreshTokenAsync(request.AdminId, request.RefreshToken);
    if (admin is null)
    {
      return null;
    }

    return await CreateTokenResponse(admin);
    
  }
  
  private async Task<TokenResponse> CreateTokenResponse(Admin admin)
  {
    return new TokenResponse()
    {
      Token = CreateToken(admin),
      RefreshToken = await GenerateAndSaveRefreshTokenAsync(admin),
      RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(5)
    };
  }
  
  private async Task<Admin> ValidateRefreshTokenAsync(int adminId, string refreshToken)
  {
    var admin = await mediator.Send(new GetAdminQuery(adminId));  
    if (admin is null || admin.Value.RefreshToken != refreshToken || admin.Value.RefreshTokenExpiryTime <= DateTime.UtcNow)
    {
      throw new Exception("Could not validate refresh token");
    }
    return admin;
  }
  
  private string GenerateRefreshToken()
  {
    byte[] randomNumber = new byte[32];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    return Convert.ToBase64String(randomNumber);
  }
  
  
  private async Task<string> GenerateAndSaveRefreshTokenAsync(Admin admin)
  {
    string refreshToken = GenerateRefreshToken();
    admin.RefreshToken = refreshToken;
    admin.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(2);
    await repository.SaveChangesAsync();
    return refreshToken;
  }
  
  private string CreateToken(Admin admin)
  {
    var claims = new List<Claim>
    {
      new(ClaimTypes.Name, admin.AdminName),
      new(ClaimTypes.NameIdentifier, admin.Id.ToString())
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
  
}
