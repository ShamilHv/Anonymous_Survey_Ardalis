namespace Anonymous_Survey_Ardalis.UseCases.Comments;

public record CommentDto(
  int CommentId,
  int SubjectId,
  string CommentText,
  DateTime CreatedAt,
  int? ParentCommentId,
  int? FileId,
  bool IsAdminComment);
