using Anonymous_Survey_Ardalis.Web.Security;
using FastEndpoints;

namespace Anonymous_Survey_Ardalis.Web.Admins.Auth.Register;

public class Register(IAuthService authService)
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
    // Option 1: Use AuthService only (recommended)
    var admin = await authService.RegisterAsync(request.AuthRequest);

    if (admin != null)
    {
      Response = new RegisterAdminResponse
      {
        AuthResponse = new AuthResponse
        {
          Admin = new AdminRecord(admin.Id, admin.AdminName, admin.Email,
            admin.SubjectId, admin.CreatedAt),
          RefreshToken = admin.RefreshToken
        }
      };
    }

    // OR Option 2: Use MediatR only (not both)
    // var result = await _mediator.Send(new CreateAdminCommand(...), cancellationToken);
    // ...rest of your code...
  }

  // public override async Task HandleAsync(
  //   RegisterAdminRequest request,
  //   CancellationToken cancellationToken)
  // {
  //   var adminRegistered = authService.RegisterAsync(request.AuthRequest);
  //
  //   var result = await _mediator.Send(new CreateAdminCommand(request.AuthRequest.AdminName,
  //     request.AuthRequest.Email, request.AuthRequest.SubjectId, request.AuthRequest.Password), cancellationToken);
  //
  //   if (result.IsSuccess)
  //   {
  //     Response = new RegisterAdminResponse
  //     {
  //       AuthResponse = new AuthResponse
  //       {
  //         Admin = new AdminRecord(result.Value.Id, result.Value.AdminName, result.Value.Email,
  //           result.Value.SubjectId, result.Value.CreatedAt),
  //         RefreshToken = result.Value.RefreshToken
  //       }
  //     };
  //   }
  //}
}
