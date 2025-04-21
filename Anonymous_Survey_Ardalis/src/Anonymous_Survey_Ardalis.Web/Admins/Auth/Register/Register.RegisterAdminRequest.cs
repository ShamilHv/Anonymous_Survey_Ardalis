using Anonymous_Survey_Ardalis.Web.Security;

namespace Anonymous_Survey_Ardalis.Web.Admins.Auth.Register;

public class RegisterAdminRequest
{
  public const string Route = "/Auth/Register";

  public AuthRequest AuthRequest { get; set; } = null!;
}
