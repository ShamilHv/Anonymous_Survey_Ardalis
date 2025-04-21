using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Admins.Get.GetById;

public class GetAdminByIdValidator : Validator<GetAdminByIdRequest>
{
  public GetAdminByIdValidator()
  {
    RuleFor(x => x.AdminId)
      .NotEmpty()
      .GreaterThan(0)
      .WithMessage("Id must be greater than zero.");
  }
}
