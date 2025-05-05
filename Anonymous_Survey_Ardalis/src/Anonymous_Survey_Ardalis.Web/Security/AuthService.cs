using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.UseCases.Admins.Queries.Get;
using Anonymous_Survey_Ardalis.UseCases.CurrentUserServices;
using Anonymous_Survey_Ardalis.UseCases.Departments.Queries.Get;
using Anonymous_Survey_Ardalis.UseCases.Subjects.Queries;
using Anonymous_Survey_Ardalis.Web.Admins;
using Anonymous_Survey_Ardalis.Web.Admins.Auth.Login;
using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Anonymous_Survey_Ardalis.Web.Security;

public class AuthService : IAuthService
{
  private readonly IConfiguration _configuration;
  private readonly ICurrentUserService _currentUserService;
  private readonly IMediator _mediator;
  private readonly IPasswordHasher<Admin> _passwordHasher;
  private readonly IAdminPermissionService _permissionService;
  private readonly IRepository<Admin> _repository;
  private readonly IEmailSender _emailSender;

  public AuthService(
    IMediator mediator,
    IRepository<Admin> repository,
    IPasswordHasher<Admin> passwordHasher,
    IConfiguration configuration,
    ICurrentUserService currentUserService,
    IAdminPermissionService permissionService,
    IEmailSender emailSender)
  {
    _mediator = mediator;
    _repository = repository;
    _passwordHasher = passwordHasher;
    _configuration = configuration;
    _currentUserService = currentUserService;
    _permissionService = permissionService;
    _emailSender = emailSender;
  }

  public async Task<AdminRecord> GetCurrentAdmin()
  {
    var admin = await _currentUserService.GetCurrentAdminEntityAsync();
    if (admin == null)
    {
      throw new Exception("Current admin could not be found");
    }

    return new AdminRecord(admin.Id, admin.AdminName, admin.Email, admin.SubjectId, admin.DepartmentId, admin.CreatedAt,
      admin.Role);
  }

  public async Task<Admin?> RegisterAsync(AuthRequest request)
  {
    var result = await _mediator.Send(new GetAdminByEmailQuery(request.Email));

    if (!result.IsNotFound())
    {
      throw new Exception("Admin with this email already exists");
    }

    // Check if current admin has permission to create new admins
    var currentAdminId = _currentUserService.GetCurrentAdminId();

    if (!await _permissionService.CanCreateAdmin(currentAdminId))
    {
      throw new Exception("Current admin does not have permission to create new admins");
    }

    var sanitizedSubjectId = request.SubjectId > 0 ? request.SubjectId : null;
    var sanitizedDepartmentId = request.DepartmentId > 0 ? request.DepartmentId : null;

    Admin newAdmin;
    
    if (sanitizedDepartmentId.HasValue)
    {
      // Verify department exists
      var departmentResult = (await _mediator.Send(new GetDepartmentQuery(sanitizedDepartmentId.Value))).Value;
      if (departmentResult is null)
      {
        throw new Exception($"Department id: {sanitizedDepartmentId.Value} is invalid");
      }

      newAdmin = Admin.CreateDepartmentAdmin(request.AdminName, request.Email, sanitizedDepartmentId.Value);
    }
    else if (sanitizedSubjectId.HasValue)
    {
      var subjectResult = (await _mediator.Send(new GetSubjectQuery(sanitizedSubjectId.Value))).Value;
      if (subjectResult is null)
      {
        throw new Exception($"Subject id: {sanitizedSubjectId.Value} is invalid");
      }

      newAdmin = Admin.CreateSubjectAdmin(request.AdminName, request.Email, sanitizedSubjectId.Value);
    }
    else
    {
      // Both IDs are null or zero - create Super Admin
      newAdmin = Admin.CreateSuperAdmin(request.AdminName, request.Email);
    }
    
    string  password = PasswordGenerator.GenerateSecurePassword();
    bool isGeneratedPassword = false;
    
    password = PasswordGenerator.GenerateSecurePassword();
    isGeneratedPassword = true;
    

    // Set password hash
    var passwordHash = _passwordHasher.HashPassword(newAdmin, password);
    newAdmin.PasswordHash = passwordHash!;

    // Save to repository
    var addedAdmin = await _repository.AddAsync(newAdmin);
    await _repository.SaveChangesAsync();

    // Send welcome email with password if it was generated
    if (isGeneratedPassword)
    {
      try
      {
        await _emailSender.SendWelcomeEmailAsync(addedAdmin, password);
      }
      catch (Exception ex)
      {
        // Log the error but don't fail the user creation
        // Consider adding proper logging here
        Console.WriteLine($"Failed to send welcome email: {ex.Message}");
      }
    }

    return addedAdmin;
  }
  
  public async Task<AuthResponse?> LoginRequestAsync(LoginRequest loginRequest)
  {
    var result = await _mediator.Send(new GetAdminByEmailQuery(loginRequest.Email));
    if (result.Status != ResultStatus.Ok || result.Value == null)
    {
      throw new Exception("Admin with this email does not exist");
    }

    var admin = result.Value;
    var passwordVerificationResult =
      _passwordHasher.VerifyHashedPassword(admin, admin.PasswordHash, loginRequest.Password);

    if (passwordVerificationResult == PasswordVerificationResult.Failed)
    {
      throw new Exception("Invalid password");
    }

    var refreshToken = GenerateRefreshToken();
    result.Value.RefreshToken = refreshToken;
    result.Value.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(2);
    await _repository.SaveChangesAsync();

    var response = new AuthResponse
    {
      Admin = new AdminRecord(admin.Id, admin.AdminName, admin.Email, admin.SubjectId, admin.DepartmentId,
        admin.CreatedAt, admin.Role),
      Token = CreateToken(admin),
      RefreshToken = refreshToken,
      RefreshTokenExpiryTime = result.Value.RefreshTokenExpiryTime
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
    var refreshToken = GenerateRefreshToken();
    var refreshTokenExpiry = DateTime.UtcNow.AddDays(2);

    admin.RefreshToken = refreshToken;
    admin.RefreshTokenExpiryTime = refreshTokenExpiry;
    await _repository.SaveChangesAsync();

    return new TokenResponse
    {
      Token = CreateToken(admin), RefreshToken = refreshToken, RefreshTokenExpiryTime = refreshTokenExpiry
    };
  }

  private async Task<Admin?> ValidateRefreshTokenAsync(int adminId, string refreshToken)
  {
    var result = await _mediator.Send(new GetAdminQuery(adminId));
    if (result.Status != ResultStatus.Ok || result.Value == null ||
        result.Value.RefreshToken != refreshToken ||
        result.Value.RefreshTokenExpiryTime <= DateTime.UtcNow)
    {
      throw new Exception("Could not validate refresh token");
    }

    return result.Value;
  }

  private string GenerateRefreshToken()
  {
    var randomNumber = new byte[32];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    return Convert.ToBase64String(randomNumber);
  }

  private string CreateToken(Admin admin)
  {
    var claims = new List<Claim>
    {
      new(ClaimTypes.Name, admin.AdminName),
      new(ClaimTypes.NameIdentifier, admin.Id.ToString()),
      new(ClaimTypes.Role, admin.Role.ToString()),
      new("SubjectId", admin.SubjectId?.ToString() ?? "0"),
      new("DepartmentId", admin.DepartmentId?.ToString() ?? "0")
    };

    var key = new SymmetricSecurityKey(
      Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

    var tokenExpiry = DateTime.UtcNow.AddHours(8);

    var tokenDescriptor = new JwtSecurityToken(
      _configuration.GetValue<string>("AppSettings:Issuer"),
      _configuration.GetValue<string>("AppSettings:Audience"),
      claims,
      expires: tokenExpiry,
      signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
  }
}
