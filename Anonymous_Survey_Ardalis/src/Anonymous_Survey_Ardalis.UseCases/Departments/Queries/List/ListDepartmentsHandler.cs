using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Departments.Queries.List;

public class ListDepartmentsHandler(IListDepartmentQueryService _query)
  : IQueryHandler<ListDepartmentsQuery, Result<IEnumerable<DepartmentDto>>>
{
  public async Task<Result<IEnumerable<DepartmentDto>>> Handle(ListDepartmentsQuery request,
    CancellationToken cancellationToken)
  {
    var result = await _query.ListAsync();
    return Result.Success(result);
  }
}
