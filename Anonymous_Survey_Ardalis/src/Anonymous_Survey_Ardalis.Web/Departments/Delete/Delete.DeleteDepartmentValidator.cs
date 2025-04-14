using Anonymous_Survey_Ardalis.Web.Subjects;
using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Departments;

public class DeleteDepartmentValidator: Validator<DeleteDepartmentRequest>
{
  public DeleteDepartmentValidator()
  {
    RuleFor(x => x.DepartmentId)
      .GreaterThan(0)
      .WithMessage("Department Id must be greater than 0");
  }
}
