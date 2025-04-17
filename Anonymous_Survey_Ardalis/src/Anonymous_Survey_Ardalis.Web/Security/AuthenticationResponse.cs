using Anonymous_Survey_Ardalis.UseCases.Admins;

namespace Anonymous_Survey_Ardalis.Web.Security;

public class AuthenticationResponse
{
  public AdminDto? Admin { get; set; }
  public string? Token { get; set; }
  public string? RefreshToken { get; set; }
  public DateTime? RefreshTokenExpiryTime { get; set; }
}
