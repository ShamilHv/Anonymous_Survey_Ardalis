using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Comments;

public class GetCommentByGuidValidator : Validator<GetCommentByGuidRequest>
{
  public GetCommentByGuidValidator()
  {
    RuleFor(x => x.CommentGuid)
      .NotEmpty()
      .WithMessage("Guid cannot be empty");
  }
}
