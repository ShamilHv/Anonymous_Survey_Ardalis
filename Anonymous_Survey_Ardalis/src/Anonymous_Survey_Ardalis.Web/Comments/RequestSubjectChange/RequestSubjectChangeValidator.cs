using FastEndpoints;
using FluentValidation;

namespace Anonymous_Survey_Ardalis.Web.Comments.RequestSubjectChange;

public class RequestSubjectChangeValidator : Validator<RequestSubjectChangeRequest>
{
  public RequestSubjectChangeValidator()
  {
    RuleFor(x => x.CommentId)
      .NotEmpty()
      .GreaterThan(0)
      .WithMessage("Comment ID is required");
            
    RuleFor(x => x.Message)
      .MaximumLength(1000)
      .WithMessage("Message cannot exceed 1000 characters");
    
  }
}
