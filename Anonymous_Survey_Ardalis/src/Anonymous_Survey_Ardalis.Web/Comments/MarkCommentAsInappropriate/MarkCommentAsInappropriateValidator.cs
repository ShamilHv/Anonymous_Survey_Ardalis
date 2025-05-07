using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Comments.MarkCommentAsInappropriate;

public class MarkCommentAsInappropriateValidator : Validator<MarkCommentAsInappropriateRequest>
{
  public MarkCommentAsInappropriateValidator()
  {
    RuleFor(x => x.CommentId)
      .GreaterThan(0)
      .WithMessage("A valid Comment ID is required");
  }
}
