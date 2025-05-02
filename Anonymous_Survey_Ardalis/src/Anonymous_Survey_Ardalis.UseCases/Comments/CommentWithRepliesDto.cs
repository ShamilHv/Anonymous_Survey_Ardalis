namespace Anonymous_Survey_Ardalis.UseCases.Comments;

public record CommentWithRepliesDto(
  int CommentId,
  int SubjectId,
  string CommentText,
  DateTime CreatedAt,
  int? ParentCommentId,
  int? FileId,
  bool IsAdminComment,
  List<CommentDto> Replies
);
