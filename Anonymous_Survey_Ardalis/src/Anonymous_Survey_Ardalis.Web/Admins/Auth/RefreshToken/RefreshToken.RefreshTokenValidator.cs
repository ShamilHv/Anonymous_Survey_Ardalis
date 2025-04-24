using Anonymous_Survey_Ardalis.Web.Security;
using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Admins.Auth.RefreshToken;

public class RefreshTokenValidator : Validator<TokenRequest>
{
  public RefreshTokenValidator()
  {
    RuleFor(t => t.AdminId)
      .NotEmpty()
      .GreaterThan(0)
      .WithMessage("Admin Id must be greater than 0");
    RuleFor(t => t.RefreshToken)
      .NotEmpty()
      .WithMessage("Refresh token cannot be empty");
  }
}
