namespace Anonymous_Survey_Ardalis.Web.Comments.Get.GetCurrent;

public class GetCurrentAdminRequest
{
  public const string Route = "/Admins/Current";

  public int AdminId { get; set; }

  public static string BuildRoute(int adminId)
  {
    return Route.Replace("{AdminId:int}", adminId.ToString());
  }
}
