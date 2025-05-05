using Ardalis.Specification;

namespace Anonymous_Survey_Ardalis.Core.CommentAggregate.Specifications;

public class CommentByFileIdSpec: Specification<Comment>
{
  public CommentByFileIdSpec(int fileId)
  {
    Query
      .Where(comment => comment.FileId == fileId)
      .Include(comment => comment.Subject);
  }
}
