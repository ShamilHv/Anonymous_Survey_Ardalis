using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.Web.Admins;
using Anonymous_Survey_Ardalis.Web.Admins.Auth.Login;

namespace Anonymous_Survey_Ardalis.Web.Security;

public interface IAuthService
{
  Task<Admin?> RegisterAsync(AuthRequest request);
  Task<AuthResponse?> LoginRequestAsync(LoginRequest loginRequest);
  Task<TokenResponse?> RefreshTokensAsync(TokenRequest request);
  Task<AdminRecord> GetCurrentAdmin();
}
