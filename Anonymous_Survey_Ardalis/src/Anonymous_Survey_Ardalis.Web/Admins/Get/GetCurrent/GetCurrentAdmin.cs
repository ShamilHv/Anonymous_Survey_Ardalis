using Anonymous_Survey_Ardalis.Web.Admins;
using Anonymous_Survey_Ardalis.Web.Security;
using FastEndpoints;

namespace Anonymous_Survey_Ardalis.Web.Comments.Get.GetCurrent;

public class GetCurrentAdminGetById(IAuthService authService) : Endpoint<EmptyRequest, AdminRecord>
{
  public override void Configure()
  {
    Get("/Admins/Current"); // Use the route directly instead of from request class
  }

  public override async Task HandleAsync(EmptyRequest request,
    CancellationToken cancellationToken)
  {
    var adminRecord = await authService.GetCurrentAdmin();

    if (adminRecord == null)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    await SendAsync(adminRecord, cancellation: cancellationToken);
  }
}
