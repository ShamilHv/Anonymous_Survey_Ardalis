
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.UseCases.Admins.Queries.Get;
using Anonymous_Survey_Ardalis.UseCases.Subjects;
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
    private readonly IMediator _mediator;
    private readonly IRepository<Admin> _repository;
    private readonly IPasswordHasher<Admin> _passwordHasher;
    private readonly IConfiguration _configuration;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAdminPermissionService _permissionService;

    public AuthService(
        IMediator mediator,
        IRepository<Admin> repository,
        IPasswordHasher<Admin> passwordHasher,
        IConfiguration configuration,
        ICurrentUserService currentUserService,
        IAdminPermissionService permissionService)
    {
        _mediator = mediator;
        _repository = repository;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
        _currentUserService = currentUserService;
        _permissionService = permissionService;
    }

    public async Task<AdminRecord> GetCurrentAdmin()
    {
      var admin = await _currentUserService.GetCurrentAdminEntityAsync();
      if (admin == null)
      {
        throw new Exception("Current admin could not be found");
      }
      return new AdminRecord(admin.Id, admin.AdminName, admin.Email, admin.SubjectId, admin.CreatedAt, admin.Role);
    }

    public async Task<Admin?> RegisterAsync(AuthRequest request)
    {
        var result = await _mediator.Send(new GetAdminByEmailQuery(request.Email));
        
        if (!result.IsNotFound())
        {
            throw new Exception("Admin with this email already exists");
        }
        
        SubjectDto? subjectResult = null;
        if (request.SubjectId is not null)
        {
            if (request.SubjectId.Value > 0)
            {
                subjectResult = (await _mediator.Send(new GetSubjectQuery(request.SubjectId))).Value;
                if (subjectResult is null)
                {
                    throw new Exception($"Subject id: {request.SubjectId} is invalid");
                }
            }
        }
        

        var currentAdminId = _currentUserService.GetCurrentAdminId();
        
        if (!await _permissionService.CanCreateAdmin(currentAdminId))
        {
            throw new Exception("Current admin does not have permission to create new admins");
        }
        
        int? departmentId = null;
        if (request.SubjectId.HasValue && subjectResult != null && 
            (request.Role == AdminRole.DepartmentAdmin))
        {
            var subject = subjectResult;
            departmentId = subject.DepartmentId;
        }
        
        var newAdmin = new Admin(request.AdminName, request.Email, request.SubjectId, request.Role)
        {
            DepartmentId = departmentId
        };
        
        var passwordHash = _passwordHasher.HashPassword(newAdmin, request.Password);
        newAdmin.PasswordHash = passwordHash!;
        newAdmin.CreatedAt = DateTime.UtcNow;
        
        var addedAdmin = await _repository.AddAsync(newAdmin);
        await _repository.SaveChangesAsync();
        
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
            Admin = new AdminRecord(admin.Id, admin.AdminName, admin.Email, admin.SubjectId, admin.CreatedAt, admin.Role),
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
            Token = CreateToken(admin),
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = refreshTokenExpiry
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
}// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Security.Cryptography;
// using System.Text;
// using Anonymous_Survey_Ardalis.Core.AdminAggregate;
// using Anonymous_Survey_Ardalis.Core.Interfaces;
// using Anonymous_Survey_Ardalis.UseCases.Admins.Queries.Get;
// using Anonymous_Survey_Ardalis.UseCases.Subjects;
// using Anonymous_Survey_Ardalis.UseCases.Subjects.Queries;
// using Anonymous_Survey_Ardalis.Web.Admins;
// using Anonymous_Survey_Ardalis.Web.Admins.Auth.Login;
// using Ardalis.Result;
// using Ardalis.SharedKernel;
// using MediatR;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.IdentityModel.Tokens;
//
// namespace Anonymous_Survey_Ardalis.Web.Security;
//
// public class AuthService(
//   IHttpContextAccessor httpContextAccessor,
//   IRepository<Admin> repository,
//   IMediator mediator,
//   IPasswordHasher<Admin> passwordHasher,
//   IConfiguration configuration) : IAuthService
//  {
//    public async Task<Admin?> RegisterAsync(AuthRequest request)
//    {
//      var result = await mediator.Send(new GetAdminByEmailQuery(request.Email));
//     
//      if (!result.IsNotFound())
//      {
//        throw new Exception("Admin with this email already exists");
//      }
//      
//      SubjectDto? subjectResult = null;
//      if (request.SubjectId is not null)
//      {
//        if (request.SubjectId.Value > 0)
//        {
//          subjectResult = (await mediator.Send(new GetSubjectQuery(request.SubjectId))).Value;
//          if (subjectResult is null)
//          {
//            throw new Exception($"Subject id: {request.SubjectId} is invalid");
//          }
//        }
//      }
//
//      var currentAdminId = GetCurrentAdminId();
//      var permissionService = httpContextAccessor.HttpContext!.RequestServices
//        .GetRequiredService<IAdminPermissionService>();
//         
//      if (!await permissionService.CanCreateAdmin(currentAdminId))
//      {
//        throw new Exception("Current admin does not have permission to create new admins");
//      }
//      // request.Role == AdminRole.SubjectAdmin
//      int? departmentId = null;
//      if (request.SubjectId.HasValue && subjectResult != null && 
//          (request.Role == AdminRole.DepartmentAdmin ))
//      {
//        var subject = subjectResult;
//        departmentId = subject.DepartmentId;
//      }
//     
//      var newAdmin = new Admin(request.AdminName, request.Email, request.SubjectId, request.Role)
//      {
//        DepartmentId = departmentId
//      };
//     
//      var passwordHash = passwordHasher.HashPassword(newAdmin, request.Password);
//      newAdmin.PasswordHash = passwordHash!;
//      newAdmin.CreatedAt = DateTime.UtcNow;
//     
//      var addedAdmin = await repository.AddAsync(newAdmin);
//      await repository.SaveChangesAsync();
//     
//      return addedAdmin;
//    }
// //   public async Task<Admin?> RegisterAsync(AuthRequest request)
// //   {
// //     var result = await mediator.Send(new GetAdminByEmailQuery(request.Email));
// //     var subjectResult = await mediator.Send(new GetSubjectQuery(request.SubjectId));
// //
// //     if (!result.IsNotFound())
// //     {
// //       throw new Exception("Admin with this email already exists");
// //     }
// //
// //     if (subjectResult is null || !subjectResult.IsSuccess)
// //     {
// //       throw new Exception($"Subject id: {request.SubjectId} is invalid");
// //     }
// //
// //     var newAdmin = new Admin(request.AdminName, request.Email, request.SubjectId);
// //     var passwordHash = passwordHasher.HashPassword(newAdmin, request.Password);
// //     newAdmin.PasswordHash = passwordHash!;
// //     newAdmin.CreatedAt = DateTime.UtcNow;
// //     var addedAdmin = await repository.AddAsync(newAdmin);
// //     await repository.SaveChangesAsync(); // Make sure this is not commented out!
// //
// //     return addedAdmin;
// //   }
//
//
//   public async Task<AdminRecord> GetCurrentAdmin()
//   {
//     var adminId = GetCurrentAdminId();
//     var result = await mediator.Send(new GetAdminQuery(adminId));
//     if (result.Status != ResultStatus.Ok || result.Value == null)
//     {
//       throw new Exception("Admin could not be found");
//     }
//
//     return new AdminRecord(result.Value.Id, result.Value.AdminName, result.Value.Email, result.Value.SubjectId,
//       result.Value.CreatedAt);
//   }
//   public async Task<AuthResponse?> LoginRequestAsync(LoginRequest loginRequest)
//   {
//     var result = await mediator.Send(new GetAdminByEmailQuery(loginRequest.Email));
//     if (result.Status != ResultStatus.Ok || result.Value == null)
//     {
//       throw new Exception("Admin with this email does not exist");
//     }
//
//     var admin = result.Value;
//     var passwordVerificationResult =
//       passwordHasher.VerifyHashedPassword(admin, admin.PasswordHash, loginRequest.Password);
//
//     if (passwordVerificationResult == PasswordVerificationResult.Failed)
//     {
//       throw new Exception("Invalid password");
//     }
//
//     var refreshToken = GenerateRefreshToken();
//     result.Value.RefreshToken = refreshToken;
//     result.Value.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(2);
//     await repository.SaveChangesAsync();
//     
//     var response = new AuthResponse
//     {
//       Admin = new AdminRecord(admin.Id, admin.AdminName, admin.Email, admin.SubjectId, admin.CreatedAt),
//       Token = CreateToken(admin),
//       RefreshToken = refreshToken,
//       RefreshTokenExpiryTime = result.Value.RefreshTokenExpiryTime // Use the same expiry time we set above
//     };
//     return response;
//   }
//  
//
//   public async Task<TokenResponse?> RefreshTokensAsync(TokenRequest request)
//   {
//     var admin = await ValidateRefreshTokenAsync(request.AdminId, request.RefreshToken);
//     if (admin is null)
//     {
//       return null;
//     }
//
//     return await CreateTokenResponse(admin);
//   }
//
//   private async Task<TokenResponse> CreateTokenResponse(Admin admin)
//   {
//     var refreshToken = GenerateRefreshToken();
//     var refreshTokenExpiry = DateTime.UtcNow.AddDays(2);
//     
//     admin.RefreshToken = refreshToken;
//     admin.RefreshTokenExpiryTime = refreshTokenExpiry;
//     await repository.SaveChangesAsync();
//     
//     return new TokenResponse
//     {
//       Token = CreateToken(admin),
//       RefreshToken = refreshToken,
//       RefreshTokenExpiryTime = refreshTokenExpiry
//     };
//   }
//
//   private async Task<Admin?> ValidateRefreshTokenAsync(int adminId, string refreshToken)
//   {
//     var result = await mediator.Send(new GetAdminQuery(adminId));
//     if (result.Status != ResultStatus.Ok || result.Value == null || 
//         result.Value.RefreshToken != refreshToken ||
//         result.Value.RefreshTokenExpiryTime <= DateTime.UtcNow)
//     {
//       throw new Exception("Could not validate refresh token");
//     }
//
//     return result.Value;
//   }
//
//   private string GenerateRefreshToken()
//   {
//     var randomNumber = new byte[32];
//     using var rng = RandomNumberGenerator.Create();
//     rng.GetBytes(randomNumber);
//     return Convert.ToBase64String(randomNumber);
//   }
//   
//   private string CreateToken(Admin admin)
//   {
//     var claims = new List<Claim>
//     {
//       new(ClaimTypes.Name, admin.AdminName), 
//       new(ClaimTypes.NameIdentifier, admin.Id.ToString()),
//       new(ClaimTypes.Role, admin.Role.ToString()),
//       new("SubjectId", admin.SubjectId?.ToString() ?? "0"),
//       new("DepartmentId", admin.DepartmentId?.ToString() ?? "0")
//     };
//   
//     var key = new SymmetricSecurityKey(
//       Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));
//     var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
//   
//     var tokenExpiry = DateTime.UtcNow.AddHours(8);
//   
//     var tokenDescriptor = new JwtSecurityToken(
//       configuration.GetValue<string>("AppSettings:Issuer"),
//       configuration.GetValue<string>("AppSettings:Audience"),
//       claims,
//       expires: tokenExpiry,
//       signingCredentials: creds
//     );
//   
//     return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
//   }
//   // private string CreateToken(Admin admin)
//   // {
//   //   var claims = new List<Claim>
//   //   {
//   //     new(ClaimTypes.Name, admin.AdminName), 
//   //     new(ClaimTypes.NameIdentifier, admin.Id.ToString())
//   //   };
//   //   
//   //   var key = new SymmetricSecurityKey(
//   //     Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));
//   //   var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
//   //   
//   //   var tokenExpiry = DateTime.UtcNow.AddHours(8);
//   //   
//   //   var tokenDescriptor = new JwtSecurityToken(
//   //     configuration.GetValue<string>("AppSettings:Issuer"),
//   //     configuration.GetValue<string>("AppSettings:Audience"),
//   //     claims,
//   //     expires: tokenExpiry,
//   //     signingCredentials: creds
//   //   );
//   //   
//   //   return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
//   // }
//   private string GetCurrentAdminName()
//   {
//     var httpContext = httpContextAccessor.HttpContext;
//     if (httpContext == null)
//     {
//       throw new InvalidOperationException("HttpContext is not available");
//     }
//
//     var adminName = httpContext.User.FindFirstValue(ClaimTypes.Name);
//
//     if (string.IsNullOrEmpty(adminName))
//     {
//       throw new Exception("Admin name claim not found in context");
//     }
//
//     return adminName;
//   }
//
//   private int GetCurrentAdminId()
//   {
//     var httpContext = httpContextAccessor.HttpContext;
//     if (httpContext == null)
//     {
//       throw new InvalidOperationException("HttpContext is not available");
//     }
//
//     var adminIdString = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
//
//     if (string.IsNullOrEmpty(adminIdString))
//     {
//       throw new Exception("Admin ID claim not found in context");
//     }
//
//     if (!int.TryParse(adminIdString, out var adminId))
//     {
//       throw new Exception("Admin ID is not in valid format");
//     }
//
//     return adminId;
//   }
// }
