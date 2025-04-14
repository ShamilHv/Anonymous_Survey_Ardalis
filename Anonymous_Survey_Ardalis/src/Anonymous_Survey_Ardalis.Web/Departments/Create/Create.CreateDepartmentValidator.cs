using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Departments;

public class CreateDepartmentValidator : Validator<CreateDepartmentRequest>
{
  public CreateDepartmentValidator()
  {
    RuleFor(x => x.DepartmentName)
      .NotEmpty()
      .WithMessage("Department name cannot be empty");
  }
}
