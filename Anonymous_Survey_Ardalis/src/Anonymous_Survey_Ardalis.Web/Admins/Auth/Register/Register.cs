using Anonymous_Survey_Ardalis.UseCases.Admins.Commands.Create;
using Anonymous_Survey_Ardalis.Web.Security;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Admins.Auth.Register;

public class Register(IMediator _mediator, IAuthService authService)
  : Endpoint<RegisterAdminRequest, RegisterAdminResponse>
{
  public override void Configure()
  {
    Post(RegisterAdminRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.ExampleRequest = new RegisterAdminRequest();
    });
  }

  public override async Task HandleAsync(
    RegisterAdminRequest request,
    CancellationToken cancellationToken)
  {
    var adminRegistered = authService.RegisterAsync(request.AuthRequest);

    var result = await _mediator.Send(new CreateAdminCommand(request.AuthRequest.AdminName,
      request.AuthRequest.Email, request.AuthRequest.SubjectId, request.AuthRequest.Password), cancellationToken);

    if (result.IsSuccess)
    {
      Response = new RegisterAdminResponse
      {
        AuthResponse = new AuthResponse
        {
          Admin = new AdminRecord(result.Value.Id, result.Value.AdminName, result.Value.Email,
            result.Value.SubjectId, result.Value.CreatedAt),
          RefreshToken = result.Value.RefreshToken
        }
      };
    }
  }
}
