using Anonymous_Survey_Ardalis.Core.DepartmentAggregate;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Departments.Queries.List;

public class ListDepartmentsHandler(IReadRepository<Department> repository)
  : IQueryHandler<ListDepartmentsQuery, Result<IEnumerable<DepartmentDto>>>
{
  public async Task<Result<IEnumerable<DepartmentDto>>> Handle(ListDepartmentsQuery request,
    CancellationToken cancellationToken)
  {
    var departments = await repository.ListAsync(cancellationToken);
    var dtoList = departments.Select(d => new DepartmentDto(
      d.Id,
      d.DepartmentName,
      d.CreatedAt
    ));
    return Result.Success(dtoList);
  }
}
