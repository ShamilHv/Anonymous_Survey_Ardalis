using Anonymous_Survey_Ardalis.Web.Security;
using FastEndpoints;

namespace Anonymous_Survey_Ardalis.Web.Admins.Auth.Login;

public class Login(IAuthService authService)
  : Endpoint<LoginRequest, AuthResponse>
{
  public override void Configure()
  {
    Post(LoginRequest.Route);
    AllowAnonymous();
    AllowFormData();
    Summary(s =>
    {
      s.ExampleRequest = new LoginRequest { Email = "admim@mail.com", Password = "Admin@123" };
    });
  }

  public override async Task HandleAsync(
    LoginRequest request,
    CancellationToken cancellationToken)
  {
    var authResponse = await authService.LoginRequestAsync(request);
    await SendAsync(authResponse!, cancellation: cancellationToken);

    // var response = new LoginResponse()
    // {
    //   AuthResponse = authResponse!
    // };
  }
}
