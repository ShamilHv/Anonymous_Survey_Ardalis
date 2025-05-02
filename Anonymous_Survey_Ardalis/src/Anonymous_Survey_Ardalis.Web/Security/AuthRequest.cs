namespace Anonymous_Survey_Ardalis.Web.Security;

public class AuthRequest
{
  public string AdminName { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  // Password is now optional - will be generated if not provided
  // public string? Password { get; set; } = null;
  public int? SubjectId { get; set; } = null;
  public int? DepartmentId { get; set; } = null;
}
