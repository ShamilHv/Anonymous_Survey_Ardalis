using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Comments.Create.CreateAdminComment;

public class CreateAdminCommentValidator:Validator<CreateAdminCommentRequest>
{
  public CreateAdminCommentValidator()
  {
    RuleFor(c=>c.CommentText)
      .NotEmpty()
      .WithMessage("Comment text cannot be empty");
    RuleFor(c=>c.ParentCommentId)
      .GreaterThan(0)
      .WithMessage("Parent comment id must be greater than 0");
  }
}
