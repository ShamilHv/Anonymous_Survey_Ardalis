namespace Anonymous_Survey_Ardalis.Web.Admins.Get.GetById;

public class GetAdminByIdRequest
{
  public const string Route = "/Admins/{AdminId:int}";

  public int AdminId { get; set; }

  public static string BuildRoute(int adminId)
  {
    return Route.Replace("{AdminId:int}", adminId.ToString());
  }
}
