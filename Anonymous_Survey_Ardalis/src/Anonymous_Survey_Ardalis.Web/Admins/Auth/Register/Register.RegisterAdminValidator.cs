using Anonymous_Survey_Ardalis.Web.Security;
using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Admins.Auth.Register;

public class RegisterAdminValidator : Validator<AuthRequest>
{
  public RegisterAdminValidator()
  {
    RuleFor(x => x.Email)
      .NotEmpty()
      .EmailAddress()
      .WithMessage("Invalid email address");  

    RuleFor(x => x.AdminName)
      .NotEmpty()
      .WithMessage("Name is required");

    RuleFor(x => x.Password)
      .NotEmpty().WithMessage("Password is required")
      .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$")
      .WithMessage(
        "Password must be at least 8 characters long and contain an uppercase letter, a lowercase letter, a number, and a special character.");
  }
}
