using Anonymous_Survey_Ardalis.Web.Admins;

namespace Anonymous_Survey_Ardalis.Web.Security;

public class AuthResponse
{
  public AdminRecord? Admin { get; set; }
  public string? Token { get; set; }
  public string? RefreshToken { get; set; }
  public DateTime? RefreshTokenExpiryTime { get; set; }
}
