using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.Core.SubjectAggregate;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.Core.CommentAggregate;

public class Comment(int subjectId, string commentText) : EntityBase, IAggregateRoot
{
  public Guid CommentIdentifier { get; set; } = Guid.NewGuid();

  public int SubjectId { get; set; } = subjectId;
  public string CommentText { get; set; } = Guard.Against.NullOrEmpty(commentText, nameof(CommentText));

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public int? ParentCommentId { get; set; }

  public int? FileId { get; set; }

  public bool IsAdminComment { get; set; } = false;

  public int? AdminId { get; set; }

  public bool IsAppropriate { get; set; } = true;

  public virtual Subject? Subject { get; set; }

  public virtual Comment? ParentComment { get; set; }

  public virtual ICollection<Comment> ChildComments { get; set; } = new List<Comment>();

  public virtual File? File { get; set; }

  public virtual Admin? Admin { get; set; }
}

public class File : IAggregateRoot
{
  public int FileId { get; set; }
  public string FilePath { get; set; } = string.Empty;
  public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
