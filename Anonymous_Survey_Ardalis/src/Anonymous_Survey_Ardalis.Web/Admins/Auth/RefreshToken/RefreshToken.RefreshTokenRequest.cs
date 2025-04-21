using Anonymous_Survey_Ardalis.Web.Security;

namespace Anonymous_Survey_Ardalis.Web.Admins.Auth.RefreshToken;

public class RefreshTokenRequest
{
  public const string Route = "/Auth/RefreshToken";

  public TokenRequest TokenRequest { get; set; } = null!;
}
