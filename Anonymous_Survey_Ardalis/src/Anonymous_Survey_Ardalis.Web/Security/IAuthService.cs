using Anonymous_Survey_Ardalis.Core.AdminAggregate;

namespace Anonymous_Survey_Ardalis.Web.Security;

public interface IAuthService
{
  Task<Admin?> RegisterAsync(AuthenticationRequest authenticationRequest);
  Task<AuthenticationResponse?> LoginRequestAsync(LoginRequest loginRequest);
  Task<TokenResponse?> RefreshTokensAsync(TokenRequest request);
}
