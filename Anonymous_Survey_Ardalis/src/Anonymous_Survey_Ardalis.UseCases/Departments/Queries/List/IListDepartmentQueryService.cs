namespace Anonymous_Survey_Ardalis.UseCases.Departments.Queries.List;

public interface IListDepartmentQueryService
{
  Task<IEnumerable<DepartmentDto>> ListAsync();
}
