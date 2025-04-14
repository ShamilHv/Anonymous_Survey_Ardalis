using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Departments;

public class GetDeparrtmentByIdValidator: Validator<GetDepartmentByIdRequest>
{
  public GetDeparrtmentByIdValidator()
  {
    RuleFor(x => x.DepartmentId)
      .GreaterThan(0)
      .WithMessage("Department Id must be greater than 0");
  }
}
