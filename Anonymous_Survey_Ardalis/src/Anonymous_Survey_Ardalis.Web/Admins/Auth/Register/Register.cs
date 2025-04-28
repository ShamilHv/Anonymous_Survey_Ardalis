using Anonymous_Survey_Ardalis.Web.Security;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Anonymous_Survey_Ardalis.Web.Admins.Auth.Register;

public class Register(IAuthService authService)
  : Endpoint<AuthRequest, RegisterAdminResponse>
{
  public override void Configure()
  {
    Post("/Auth/Register");
    AllowFormData();
    Summary(s =>
    {
      s.ExampleRequest = new AuthRequest
      {
        AdminName = "Admin", Email = "admin@example.com", Password = "password", SubjectId = 1
      };
    });
  }

  [Authorize(Policy = "SuperAdminOnly")]
  public override async Task HandleAsync(
    AuthRequest request,
    CancellationToken cancellationToken)
  {
    var admin = await authService.RegisterAsync(request);

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
  }
}
