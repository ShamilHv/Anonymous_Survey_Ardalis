using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Subjects;

public class GetSubjectByIdValidator : Validator<GetSubjectByIdRequest>
{
  public GetSubjectByIdValidator()
  {
    RuleFor(x => x.SubjectId)
      .GreaterThan(0)
      .WithMessage("Subject Id must be greater than 0");
  }
}
