using Ardalis.Specification;

namespace Anonymous_Survey_Ardalis.Core.CommentAggregate.Specifications;

public class CommentWithRepliesSpec : Specification<Comment>
{
  public CommentWithRepliesSpec(int commentId)
  {
    Query
      .Where(c => c.ParentCommentId == commentId)
      .Include(c => c.ChildComments);
  }
}
