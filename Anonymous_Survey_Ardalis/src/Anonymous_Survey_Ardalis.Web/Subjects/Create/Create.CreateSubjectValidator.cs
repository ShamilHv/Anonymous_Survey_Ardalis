using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Subjects;

public class CreateSubjectValidator : Validator<CreateSubjectRequest>
{
  public CreateSubjectValidator()
  {
    RuleFor(x => x.SubjectName)
      .NotEmpty()
      .WithMessage("Subject Name is required.");
    RuleFor(x => x.DepartmentId)
      .NotEmpty()
      .WithMessage("Department Id is required.");
  }
}
