using Anonymous_Survey_Ardalis.UseCases.Departments;
using Anonymous_Survey_Ardalis.UseCases.Departments.Queries.List;

namespace Anonymous_Survey_Ardalis.Infrastructure.Data.Queries;

public class ListDepartmentsQueryService : IListDepartmentQueryService
{
  public Task<IEnumerable<DepartmentDto>> ListAsync()
  {
    List<DepartmentDto> fakeDepartments =
    [
      new(1, "IT", DateTime.Now),
      new(1, "Finance", DateTime.Now.AddDays(-2).AddHours(-3))
    ];

    return Task.FromResult(fakeDepartments.AsEnumerable());
  }
}
