using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Comments.ReportInappropriateComment;

public class ReportInappropriateCommentValidator : Validator<ReportInappropriateCommentRequest>
{
  public ReportInappropriateCommentValidator()
  {
    RuleFor(x => x.CommentId)
      .GreaterThan(0)
      .WithMessage("Comment ID must be greater than 0");

    RuleFor(x => x.Message)
      .MaximumLength(1000)
      .WithMessage("Message can not exceed 1000 characters");
  }
}
