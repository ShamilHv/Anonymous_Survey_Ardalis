using Anonymous_Survey_Ardalis.Core.AdminAggregate;

namespace Anonymous_Survey_Ardalis.Web.Security;

  public class AuthRequest
  {
    public string AdminName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int? SubjectId { get; set; } = null;
    public AdminRole Role { get; set; } = AdminRole.SubjectAdmin;
  }

