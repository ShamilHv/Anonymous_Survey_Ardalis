using Ardalis.Specification;

namespace Anonymous_Survey_Ardalis.Core.CommentAggregate.Specifications;

public class CommentByGuidSpec : Specification<Comment>
  {
  public CommentByGuidSpec(Guid commentIdentifier)
  {
    Query
      .Where(comment => comment.CommentIdentifier == commentIdentifier);
  }
}
