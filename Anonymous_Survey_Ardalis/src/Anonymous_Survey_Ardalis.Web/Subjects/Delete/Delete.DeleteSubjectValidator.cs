using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Subjects;

public class DeleteSubjectValidator : Validator<DeleteSubjectRequest>
{
  public DeleteSubjectValidator()
  {
    RuleFor(x => x.Subjectid)
      .GreaterThan(0)
      .WithMessage("Id must be greater than zero.");
  }
}
