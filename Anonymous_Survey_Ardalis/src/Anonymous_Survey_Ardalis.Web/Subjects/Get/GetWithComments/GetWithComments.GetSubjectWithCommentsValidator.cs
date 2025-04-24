using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Subjects.Get.GetWithComments;

public class GetSubjectWithCommentsValidator : Validator<GetSubjectWithCommentsRequest>
{
  public GetSubjectWithCommentsValidator()
  {
    RuleFor(x => x.SubjectId)
      .GreaterThan(0)
      .WithMessage("Department Id must be greater than 0");
  }
}
