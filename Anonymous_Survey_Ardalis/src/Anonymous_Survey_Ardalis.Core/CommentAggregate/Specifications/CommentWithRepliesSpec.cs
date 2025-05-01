using Ardalis.Specification;

namespace Anonymous_Survey_Ardalis.Core.CommentAggregate.Specifications;

public class CommentWithRepliesSpec : Specification<Comment>
{
  public CommentWithRepliesSpec(Guid commentIdentifier)
  {
    Query
      .Where(c => c.ParentComment != null && c.ParentComment.CommentIdentifier == commentIdentifier)
      .Include(c => c.ChildComments);
  }
}
