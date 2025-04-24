using Anonymous_Survey_Ardalis.UseCases.Comments;
using Anonymous_Survey_Ardalis.UseCases.Comments.Queries.Get.GetWithReplies;
using Ardalis.Result;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Comments.Get.GetWithReplies;

public class GetWithReplies(IMediator _mediator)
  : Endpoint<GetCommentWithRepliesRequest, CommentWithRepliesDto>
{
  public override void Configure()
  {
    Get(GetCommentWithRepliesRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(GetCommentWithRepliesRequest request,
    CancellationToken cancellationToken)
  {
    var query = new GetCommentWithRepliesQuery(request.CommentId);

    var result = await _mediator.Send(query, cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      Response = result;
    }
  }
}
