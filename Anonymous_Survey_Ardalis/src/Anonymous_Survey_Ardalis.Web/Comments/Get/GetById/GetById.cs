using Anonymous_Survey_Ardalis.UseCases.Comments.Queries.Get;
using Ardalis.Result;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Comments;

public class GetById(IMediator _mediator)
  : Endpoint<GetCommentByIdRequest, CommentRecord>
{
  public override void Configure()
  {
    Get(GetCommentByIdRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(GetCommentByIdRequest request,
    CancellationToken cancellationToken)
  {
    var query = new GetCommentQuery(request.CommentId);

    var result = await _mediator.Send(query, cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      Response = new CommentRecord(result.Value.CommentId, result.Value.CommentText, result.Value.SubjectId);
    }
  }
}
