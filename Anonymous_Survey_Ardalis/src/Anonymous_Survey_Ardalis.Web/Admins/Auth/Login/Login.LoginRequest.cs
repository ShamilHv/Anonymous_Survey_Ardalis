namespace Anonymous_Survey_Ardalis.Web.Admins.Auth.Login;

public class LoginRequest
{
  public const string Route = "/Auth/Login";

  public string Email { get; set; } = null!;
  public string Password { get; set; } = null!;
}
