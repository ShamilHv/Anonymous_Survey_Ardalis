using Anonymous_Survey_Ardalis.UseCases.Comments.Queries.List;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Comments;

public class List(IMediator _mediator) : EndpointWithoutRequest<CommentListResponse>
{
  public override void Configure()
  {
    Get("/Comments");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken cancellationToken)
  {
    var result =
      await _mediator.Send(new ListCommentsQuery(), cancellationToken);

    if (result.IsSuccess)
    {
      Response = new CommentListResponse
      {
        Comments = result.Value.Select(c => new CommentRecord(c.CommentId, c.CommentText, c.SubjectId)).ToList()
      };
    }
  }
}
