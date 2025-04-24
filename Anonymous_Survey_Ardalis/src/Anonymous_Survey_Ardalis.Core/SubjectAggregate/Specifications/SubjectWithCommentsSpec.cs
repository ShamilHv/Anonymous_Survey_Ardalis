using Ardalis.Specification;

namespace Anonymous_Survey_Ardalis.Core.SubjectAggregate.Specifications;

public class SubjectWithCommentsSpec : Specification<Subject>
{
  public SubjectWithCommentsSpec(int subjectId)
  {
    Query
      .Where(s => s.Id == subjectId).Include(d => d.Comments);
  }
}
