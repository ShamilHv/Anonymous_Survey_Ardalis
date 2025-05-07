namespace Anonymous_Survey_Ardalis.Web.Comments.Get.GetById;

public class GetCommentByIdRequest
{
  public const string Route = "/Comments/{CommentId:int}";
  public int CommentId { get; set; }
}
