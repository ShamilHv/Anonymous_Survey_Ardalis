namespace Anonymous_Survey_Ardalis.UseCases.Comments;

public record CommentDto(
  int commentId,
  int subjectId,
  string commentText,
  DateTime createdAt,
  int? parentCommentId,
  string? filePath,
  bool isAdminComment);
