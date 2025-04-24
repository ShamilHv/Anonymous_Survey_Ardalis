using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Comments;

public class GetCommentByIdValidator : Validator<GetCommentByIdRequest>
{
  public GetCommentByIdValidator()
  {
    RuleFor(x => x.CommentId)
      .NotEmpty()
      .GreaterThan(0)
      .WithMessage("Id must be greater than zero.");
  }
}
