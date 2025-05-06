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
  }
}
