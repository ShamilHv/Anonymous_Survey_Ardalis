using Anonymous_Survey_Ardalis.UseCases.Admins.Commands.Create;
using Anonymous_Survey_Ardalis.Web.Admins.Auth.Register;
using Anonymous_Survey_Ardalis.Web.Security;
using Azure;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Admins.Auth.Login;

public class Login(IAuthService authService)
  : Endpoint<LoginRequest, LoginResponse>
{
  public override void Configure()
  {
    Post(LoginRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.ExampleRequest = new LoginRequest();
    });
  }

  public override async Task HandleAsync(
    LoginRequest request,
    CancellationToken cancellationToken)
  {
    var authResponse =await authService.LoginRequestAsync(request);

    var response = new LoginResponse()
    {
      AuthResponse = authResponse!
    };
  }
}
