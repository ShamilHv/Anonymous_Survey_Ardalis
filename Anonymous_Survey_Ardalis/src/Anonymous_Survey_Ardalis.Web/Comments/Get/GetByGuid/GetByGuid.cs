using System.Security.Claims;
using Anonymous_Survey_Ardalis.UseCases.Comments.Queries.Get.GetByGuid;
using Ardalis.Result;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Comments;

public class GetByGuid : Endpoint<GetCommentByGuidRequest, CommentRecord>
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IMediator _mediator;

  public GetByGuid(IMediator mediator, IHttpContextAccessor httpContextAccessor)
  {
    _mediator = mediator;
    _httpContextAccessor = httpContextAccessor;
  }

  public override void Configure()
  {
    Get(GetCommentByGuidRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(GetCommentByGuidRequest request,
    CancellationToken cancellationToken)
  {
    var query = new GetCommentByGuidQuery(request.CommentGuid);

    var result = await _mediator.Send(query, cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      if (result.Errors.Any())
      {
        await SendNotFoundAsync();
      }
      else
      {
        await SendNotFoundAsync(cancellationToken);
      }

      return;
    }

    if (result.IsSuccess)
    {
      var response = new CommentRecord(
        result.Value.CommentId,
        result.Value.CommentText,
        result.Value.SubjectId);
      await SendOkAsync(response, cancellationToken);
      return;
    }

    await SendErrorsAsync(1);
  }
}
