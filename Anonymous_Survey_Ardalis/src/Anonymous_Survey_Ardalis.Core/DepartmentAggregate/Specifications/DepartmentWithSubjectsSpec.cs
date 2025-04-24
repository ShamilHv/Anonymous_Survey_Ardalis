using Ardalis.Specification;

namespace Anonymous_Survey_Ardalis.Core.DepartmentAggregate.Specifications;

public class DepartmentWithSubjectsSpec : Specification<Department>
{
  public DepartmentWithSubjectsSpec(int departmentId)
  {
    Query
      .Where(d => d.Id == departmentId).Include(d => d.Subjects);
  }
}
