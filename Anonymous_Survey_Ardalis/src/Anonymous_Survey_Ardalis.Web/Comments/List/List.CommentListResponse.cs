namespace Anonymous_Survey_Ardalis.Web.Comments;

public class CommentListResponse
{
  public List<CommentRecord> Comments { get; set; } = [];
  public int PageNumber { get; set; }
  public int PageSize { get; set; }
  public int TotalPages { get; set; }
  public int TotalCount { get; set; }
  public bool HasPreviousPage => PageNumber > 1;
  public bool HasNextPage => PageNumber < TotalPages;
}

public record CommentListRequest
{
  public int? SubjectId { get; init; }
  public int? DepartmentId { get; init; }
  public int PageNumber { get; init; } = 1;
  public int PageSize { get; init; } = 10;
}
