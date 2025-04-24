using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Admins.Auth.RefreshToken;

public class RefreshTokenValidator : Validator<RefreshTokenRequest>
{
  public RefreshTokenValidator()
  {
    RuleFor(t => t.TokenRequest.AdminId)
      .GreaterThan(0)
      .WithMessage("Admin Id must be greater than 0");
    RuleFor(t => t.TokenRequest.RefreshToken)
      .NotEmpty()
      .WithMessage("Refresh token cannot be empty");
  }
}
