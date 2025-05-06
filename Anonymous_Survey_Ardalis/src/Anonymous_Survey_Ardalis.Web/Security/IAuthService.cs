using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.Web.Admins;
using Anonymous_Survey_Ardalis.Web.Admins.Auth.Login;

namespace Anonymous_Survey_Ardalis.Web.Security;

public interface IAuthService
{
  Task<AdminRecord> GetCurrentAdmin();

  /// <summary>
  ///   Registers a new admin. If no password is provided, a random one will be generated
  ///   and emailed to the new admin.
  /// </summary>
  Task<Admin?> RegisterAsync(AuthRequest request);

  Task<AuthResponse?> LoginRequestAsync(LoginRequest loginRequest);

  Task<TokenResponse?> RefreshTokensAsync(TokenRequest request);
}
