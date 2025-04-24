namespace Anonymous_Survey_Ardalis.Web.Admins.Get.GetByEmail;

public class GetAdminByEmailRequest
{
  public const string Route = "/Admins/ByEmail/{AdminEmail}";

  public string AdminEmail { get; set; } = string.Empty;

  public static string BuildRoute(string adminEmail)
  {
    return Route.Replace("{AdminEmail}", adminEmail);
  }
}
