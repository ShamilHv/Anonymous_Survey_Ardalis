using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Comments.UpdateSubject;

public class UpdateCommentSubjectValidator : Validator<UpdateCommentSubjectRequest>
{
  public UpdateCommentSubjectValidator()
  {
    RuleFor(x => x.CommentId)
      .GreaterThan(0)
      .WithMessage("Comment ID must be provided and greater than 0");

    RuleFor(x => x.NewSubjectId)
      .GreaterThan(0)
      .WithMessage("New Subject ID must be provided and greater than 0");
  }
}
