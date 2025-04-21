using Anonymous_Survey_Ardalis.Web.Admins.Auth.Login;
using Anonymous_Survey_Ardalis.Web.Security;
using FastEndpoints;

namespace Anonymous_Survey_Ardalis.Web.Admins.Auth.RefreshToken;

public class RefreshToken(IAuthService authService)
  : Endpoint<RefreshTokenRequest, RefreshTokenResponse>
{
  public override void Configure()
  {
    Post(RefreshTokenRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.ExampleRequest = new RefreshTokenRequest();
    });
  }

  public override async Task HandleAsync(
    RefreshTokenRequest request,
    CancellationToken cancellationToken)
  {
    var tokenResponse =await authService.RefreshTokensAsync(request.TokenRequest);

    var response = new RefreshTokenResponse()
    {
      TokenResponse = tokenResponse!
    };
  }
}
