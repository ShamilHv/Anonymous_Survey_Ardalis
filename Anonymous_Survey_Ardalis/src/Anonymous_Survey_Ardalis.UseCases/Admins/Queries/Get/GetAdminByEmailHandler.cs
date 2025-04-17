using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.Core.AdminAggregate.Specifications;
using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;

namespace Anonymous_Survey_Ardalis.UseCases.Admins.Queries.Get;

public class GetAdminByEmailHandler(IReadRepository<Admin> repository)
  : IRequestHandler<GetAdminByEmailQuery, Result<Admin>>
{
  public async Task<Result<Admin>> Handle(GetAdminByEmailQuery request, CancellationToken cancellationToken)
  {
    var spec = new AdminByEmailSpec(request.Email);
    var admin = await repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (admin == null)
    {
      return Result.NotFound();
    }

    return Result<Admin>.Success(admin);
  }
}
