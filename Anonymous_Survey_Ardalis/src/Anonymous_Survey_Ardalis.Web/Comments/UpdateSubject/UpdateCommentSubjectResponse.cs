namespace Anonymous_Survey_Ardalis.Web.Comments.UpdateSubject;

public class UpdateCommentSubjectResponse
{
  public UpdateCommentSubjectResponse(bool success, string message = "")
  {
    Success = success;
    Message = message;
  }

  public bool Success { get; set; }
  public string Message { get; set; }
}
