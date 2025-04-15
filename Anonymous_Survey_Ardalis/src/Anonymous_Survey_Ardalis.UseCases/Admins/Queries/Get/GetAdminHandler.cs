using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.Core.AdminAggregate.Specifications;
using Anonymous_Survey_Ardalis.Core.DepartmentAggregate.Specifications;
using Anonymous_Survey_Ardalis.UseCases.Departments;
using Anonymous_Survey_Ardalis.UseCases.Departments.Queries.Get;
using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;

namespace Anonymous_Survey_Ardalis.UseCases.Admins.Queries.Get;

public class GetAdminHandler(IReadRepository<Admin> repository)
  : IRequestHandler<GetAdminQuery, Result<AdminDto>>
{
  public async Task<Result<AdminDto>> Handle(GetAdminQuery request, CancellationToken cancellationToken)
  {
    var spec = new AdminByIdSpec(request.Id);
    var admin = await repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (admin == null)
    {
      return Result.NotFound();
    }

    return new AdminDto(admin.Id,admin.AdminName, admin.Email, admin.SubjectId, admin.CreatedAt);
  }
}
