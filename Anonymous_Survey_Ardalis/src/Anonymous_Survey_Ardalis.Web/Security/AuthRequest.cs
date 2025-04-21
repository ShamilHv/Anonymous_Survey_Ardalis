namespace Anonymous_Survey_Ardalis.Web.Security;

public class AuthRequest
{
  public string AdminName { get; set; } = null!;
  public string Email { get; set; } = null!;
  public int SubjectId { get; set; }
  public string Password { get; set; } = null!;
}
