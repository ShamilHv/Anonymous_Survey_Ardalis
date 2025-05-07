using Ardalis.Result;
using MediatR;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Commands.ReportInappropriateComment;

public class ReportInappropriateCommentCommand : IRequest<Result<bool>>
{
  public int CommentId { get; set; }
  public string? Message { get; set; }
}
