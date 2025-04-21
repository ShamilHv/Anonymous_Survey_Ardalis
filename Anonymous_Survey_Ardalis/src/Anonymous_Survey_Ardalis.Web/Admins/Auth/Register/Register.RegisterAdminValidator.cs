using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Admins.Auth.Register;

public class RegisterAdminValidator : Validator<RegisterAdminRequest>
{
  public RegisterAdminValidator()
  {
    RuleFor(x => x.AuthRequest.Email)
      .EmailAddress()
      .WithMessage("Invalid email address");
    RuleFor(x => x.AuthRequest.AdminName)
      .NotEmpty()
      .WithMessage("Name is required");
    RuleFor(x => x.AuthRequest.SubjectId)
      .GreaterThan(0)
      .WithMessage("Invalid Subject Id");
    RuleFor(x => x.AuthRequest.Password)
      .NotEmpty().WithMessage("Password is required")
      .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$")
      .WithMessage(
        "Password must be at least 8 characters long and contain an uppercase letter, a lowercase letter, a number, and a special character.");
  }
}
