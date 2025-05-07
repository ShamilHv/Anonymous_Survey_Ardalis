using System.Linq.Expressions;
using Anonymous_Survey_Ardalis.UseCases.Comments.Queries.Get;
using Anonymous_Survey_Ardalis.UseCases.CurrentUserServices;
using Ardalis.Result;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Comments.Get.GetById;

public class GetById : Endpoint<GetCommentByIdRequest, CommentRecord>
{
  private readonly ICurrentUserService _currentUserService;
  private readonly IMediator _mediator;

  public GetById(IMediator mediator, ICurrentUserService currentUserService)
  {
    _mediator = mediator;
    _currentUserService = currentUserService;
  }

  public override void Configure()
  {
    Get(GetCommentByIdRequest.Route);
  }

  public override async Task HandleAsync(GetCommentByIdRequest request,
    CancellationToken cancellationToken)
  {

      var adminId = _currentUserService.GetCurrentAdminId();

      var query = new GetCommentQuery(request.CommentId, adminId);
      var result = await _mediator.Send(query, cancellationToken);
      if (result.Status == ResultStatus.NotFound)
      {
        await SendNotFoundAsync(cancellationToken);
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
