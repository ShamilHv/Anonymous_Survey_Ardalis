using Ardalis.Result;
using MediatR;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Commands.MarkAsInappropriate;

public class MarkCommentAsInappropriateCommand: IRequest<Result<bool>>
{
  public int CommentId { get; set; }
}
