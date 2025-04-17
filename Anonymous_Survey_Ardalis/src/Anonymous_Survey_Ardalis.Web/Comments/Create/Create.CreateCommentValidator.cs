using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Comments;

public class CreateCommentValidator : Validator<CreateCommentRequest>
{
  public CreateCommentValidator()
  {
    RuleFor(x => x.SubjectId)
      .NotEmpty()
      .WithMessage("Subject Id is required.");
    RuleFor(x => x.CommentText)
      .NotEmpty()
      .WithMessage("Comment Text is required.");
  }
}
