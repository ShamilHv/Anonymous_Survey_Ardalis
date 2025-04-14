using Ardalis.Specification;

namespace Anonymous_Survey_Ardalis.Core.SubjectAggregate.Specifications;

public class SubjectByIdSpec : Specification<Subject>
{
  public SubjectByIdSpec(int subjectId)
  {
    Query
      .Where(subject => subject.Id == subjectId);
  }
}
