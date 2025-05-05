using Ardalis.Result;
using MediatR;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Commands.RequestSubjectChange;

public class RequestSubjectChangeCommand: IRequest<Result<bool>>
{
  public int CommentId { get; set; }
  public int? SuggestedSubjectId { get; set; }
  public string? Message { get; set; }
}
