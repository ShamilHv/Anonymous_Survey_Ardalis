using Anonymous_Survey_Ardalis.Web.Security;
using FastEndpoints;

namespace Anonymous_Survey_Ardalis.Web.Admins.Auth.RefreshToken;

public class RefreshToken(IAuthService authService)
  : Endpoint<TokenRequest, RefreshTokenResponse>
{
  public override void Configure()
  {
    Post("/Auth/RefreshToken");
    AllowAnonymous();
    AllowFormData();
    Summary(s =>
    {
      s.ExampleRequest = new TokenRequest(20, "Token");
    });
  }

  public override async Task HandleAsync(
    TokenRequest request,
    CancellationToken cancellationToken)
  {
    var tokenResponse = await authService.RefreshTokensAsync(request);

    var response = new RefreshTokenResponse { TokenResponse = tokenResponse! };
    await SendAsync(response!, cancellation: cancellationToken);
  }
}
