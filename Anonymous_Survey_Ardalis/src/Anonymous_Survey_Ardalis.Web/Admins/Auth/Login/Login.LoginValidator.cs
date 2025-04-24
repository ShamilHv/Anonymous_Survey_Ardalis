using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Admins.Auth.Login;

public class LoginValidator : Validator<LoginRequest>
{
  public LoginValidator()
  {
    RuleFor(a => a.Email)
      .EmailAddress()
      .WithMessage("Invalid Email Address");
    RuleFor(a => a.Password)
      .NotEmpty()
      .WithMessage("Password can not be empty");
  }
}
