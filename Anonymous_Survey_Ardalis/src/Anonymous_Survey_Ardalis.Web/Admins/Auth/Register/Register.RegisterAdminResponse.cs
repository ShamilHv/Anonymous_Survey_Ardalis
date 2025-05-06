using Anonymous_Survey_Ardalis.Web.Security;

namespace Anonymous_Survey_Ardalis.Web.Admins.Auth.Register;

public class RegisterAdminResponse
{
  public AuthResponse? AuthResponse { get; set; }

  public string? Message { get; set; }
}
