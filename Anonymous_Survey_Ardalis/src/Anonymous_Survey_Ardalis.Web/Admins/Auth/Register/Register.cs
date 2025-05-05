using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.UseCases.CurrentUserServices;
using Anonymous_Survey_Ardalis.Web.Security;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Anonymous_Survey_Ardalis.Web.Admins.Auth.Register;

public class Register(
  IAuthService authService,
  IAdminPermissionService _adminPermissionService,
  ICurrentUserService _currentUserService)
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
        AdminName = "Admin", 
        Email = "admin@example.com", 
        // Password is now optional
        SubjectId = 1
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

    // Password will be generated in the service if not provided
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
        },
        // Add a message if password was generated
        Message = "Admin created successfully. A temporary password has been sent to their email." 
      };
    }
  }
}
