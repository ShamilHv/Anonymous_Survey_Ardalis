using Anonymous_Survey_Ardalis.Core.DepartmentAggregate;
using Anonymous_Survey_Ardalis.Core.DepartmentAggregate.Specifications;
using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;

namespace Anonymous_Survey_Ardalis.UseCases.Departments.Queries.Get;

public class GetDepartmentHandler(IReadRepository<Department> repository)
  : IRequestHandler<GetDepartmentQuery, Result<DepartmentDto>>
{
  public async Task<Result<DepartmentDto>> Handle(GetDepartmentQuery request, CancellationToken cancellationToken)
  {
    var spec = new DepartmentByIdSpec(request.Id);
    var department = await repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (department == null)
    {
      return Result.NotFound();
    }

    return new DepartmentDto(department.Id, department.DepartmentName, DateTime.UtcNow);
  }
}
