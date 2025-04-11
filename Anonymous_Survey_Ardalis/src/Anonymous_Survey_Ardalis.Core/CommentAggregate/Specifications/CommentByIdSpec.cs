using Ardalis.Specification;

namespace Anonymous_Survey_Ardalis.Core.CommentAggregate.Specifications;

public class CommentByIdSpec : Specification<Comment>
{
  public CommentByIdSpec(int commentId)
  {
    Query
      .Where(comment => comment.Id == commentId);
  }
}
