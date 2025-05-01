using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Ardalis.Specification;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Queries.List;

public class CommentPaginatedSpecification: Specification<Comment>
{
  public CommentPaginatedSpecification(int pageNumber, int pageSize, int? subjectId = null)
  {
    if (subjectId.HasValue)
    {
      Query.Where(c => c.SubjectId == subjectId.Value);
    }
        
    Query.Skip((pageNumber - 1) * pageSize)
      .Take(pageSize)
      .OrderByDescending(c => c.CreatedAt);
  }
}
