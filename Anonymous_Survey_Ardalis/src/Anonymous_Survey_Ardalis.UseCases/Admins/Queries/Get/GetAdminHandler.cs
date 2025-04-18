using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.Core.AdminAggregate.Specifications;
using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;

namespace Anonymous_Survey_Ardalis.UseCases.Admins.Queries.Get;

public class GetAdminHandler(IReadRepository<Admin> repository)
  : IRequestHandler<GetAdminQuery, Result<Admin>>
{
  public async Task<Result<Admin>> Handle(GetAdminQuery request, CancellationToken cancellationToken)
  {
    var spec = new AdminByIdSpec(request.Id);
    var admin = await repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (admin == null)
    {
      return Result.NotFound();
    }

    return admin;
  }
}
