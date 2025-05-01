using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.UseCases.Admins.Queries.Get;
using Ardalis.Result;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Admins.Get.GetByEmail;

public class GetByEmail(IMediator mediator) : Endpoint<GetAdminByEmailRequest, AdminRecord>
{
  public override void Configure()
  {
    Get(GetAdminByEmailRequest.Route); 
  }

  public override async Task HandleAsync(GetAdminByEmailRequest request,
    CancellationToken cancellationToken)
  {
    var query = new GetAdminByEmailQuery(request.AdminEmail);

    var result = await mediator.Send(query, cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      Response = new AdminRecord(result.Value.Id, result.Value.AdminName, result.Value.Email, result.Value.SubjectId,
        result.Value.DepartmentId, result.Value.CreatedAt, result.Value.Role);
    }
  }
}



