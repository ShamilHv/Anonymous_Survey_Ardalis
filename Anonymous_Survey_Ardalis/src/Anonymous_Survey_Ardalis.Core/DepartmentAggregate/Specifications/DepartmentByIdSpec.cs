using Ardalis.Specification;

namespace Anonymous_Survey_Ardalis.Core.DepartmentAggregate.Specifications;

public class DepartmentByIdSpec : Specification<Department>
{
  public DepartmentByIdSpec(int departmentId)
  {
    Query
      .Where(department => department.Id == departmentId);
  }
}
