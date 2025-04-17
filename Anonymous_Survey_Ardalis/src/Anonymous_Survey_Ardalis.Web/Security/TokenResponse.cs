namespace Anonymous_Survey_Ardalis.Web.Security;

public class TokenResponse
{
  public string? Token { get; set; }
  public string? RefreshToken { get; set; }
  public DateTime? RefreshTokenExpiryTime { get; set; }
}
