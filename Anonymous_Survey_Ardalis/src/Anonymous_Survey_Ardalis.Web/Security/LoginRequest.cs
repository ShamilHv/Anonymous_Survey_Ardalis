using System.ComponentModel.DataAnnotations;

namespace Anonymous_Survey_Ardalis.Web.Security;

public class LoginRequest
{
  [Required]
  [MaxLength(255)]
  [EmailAddress]
  public string Email { get; set; } = "";

  [Required]
  [MaxLength(255)] 
  public string Password { get; set; } = "";
}
