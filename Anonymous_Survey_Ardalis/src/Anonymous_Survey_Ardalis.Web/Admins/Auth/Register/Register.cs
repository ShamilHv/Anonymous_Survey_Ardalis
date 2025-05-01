using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.Web.Security;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Anonymous_Survey_Ardalis.Web.Admins.Auth.Register;

public class Register(IAuthService authService, IAdminPermissionService _adminPermissionService, ICurrentUserService _currentUserService)
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
    var adminId = _currentUserService.GetCurrentAdminId();
    if (!await _adminPermissionService.CanCreateAdmin(adminId))
    {
      await SendForbiddenAsync();
      return;
    }
    var admin = await authService.RegisterAsync(request);

    if (admin != null)
    {
      Response = new RegisterAdminResponse
      {
        AuthResponse = new AuthResponse
        {
          Admin = new AdminRecord(admin.Id, admin.AdminName, admin.Email,
            admin.SubjectId, admin.DepartmentId, admin.CreatedAt, admin.Role),
          RefreshToken = admin.RefreshToken
        }
      };
    }
  }
}
