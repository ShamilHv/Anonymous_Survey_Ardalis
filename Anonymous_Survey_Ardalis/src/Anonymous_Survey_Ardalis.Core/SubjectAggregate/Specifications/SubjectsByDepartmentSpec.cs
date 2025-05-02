using Ardalis.Specification;

namespace Anonymous_Survey_Ardalis.Core.SubjectAggregate.Specifications;

public class SubjectsByDepartmentSpec : Specification<Subject>
{
  public SubjectsByDepartmentSpec(int departmentId)
  {
    Query.Where(subject => subject.DepartmentId == departmentId);
  }
}
