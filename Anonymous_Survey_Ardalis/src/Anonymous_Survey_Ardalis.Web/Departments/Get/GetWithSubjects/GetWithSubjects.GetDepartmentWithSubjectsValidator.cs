using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Departments.Get.GetWithSubjects;

public class GetDepartmentWithSubjectsValidator : Validator<GetDepartmentWithSubjectsRequest>
{
  public GetDepartmentWithSubjectsValidator()
  {
    RuleFor(x => x.DepartmentId)
      .GreaterThan(0)
      .WithMessage("Department Id must be greater than 0");
  }
}
