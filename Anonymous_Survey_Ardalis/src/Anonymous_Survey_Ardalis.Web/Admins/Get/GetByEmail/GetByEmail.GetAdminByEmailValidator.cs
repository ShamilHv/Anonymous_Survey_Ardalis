using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Admins.Get.GetByEmail;

public class GetAdminByEmailValidator: Validator<GetAdminByEmailRequest>
{
  public GetAdminByEmailValidator()
  {
    RuleFor(x => x.AdminEmail)
      .NotEmpty().WithMessage("Email is required.")
      .EmailAddress().WithMessage("Invalid email format.");
  }
}
