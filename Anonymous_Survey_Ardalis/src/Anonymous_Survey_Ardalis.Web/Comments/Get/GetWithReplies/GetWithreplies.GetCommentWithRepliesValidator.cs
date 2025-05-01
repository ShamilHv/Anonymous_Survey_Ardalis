using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Comments.Get.GetWithReplies;

public class GetCommentWithRepliesValidator : Validator<GetCommentWithRepliesRequest>
{
  public GetCommentWithRepliesValidator()
  {
    RuleFor(x => x.CommentGuid)
      .NotEmpty()
      .WithMessage("Comment Guid cannot be empty");
  }
}
