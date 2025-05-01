using Anonymous_Survey_Ardalis.UseCases.Comments.Queries.Get;
using Anonymous_Survey_Ardalis.UseCases.Comments.Queries.Get.GetByGuid;
using Ardalis.Result;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Comments;

public class GetByGuid(IMediator _mediator)
  : Endpoint<GetCommentByGuidRequest, CommentRecord>
{
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
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      var response = new CommentRecord(result.Value.CommentId, result.Value.CommentText, result.Value.SubjectId);
      await SendOkAsync(response, cancellationToken);
      return;
    }
    
    await SendErrorsAsync(1);
  }
}
// using Anonymous_Survey_Ardalis.UseCases.Comments.Queries.Get;
// using Anonymous_Survey_Ardalis.UseCases.Comments.Queries.Get.GetByGuid;
// using Ardalis.Result;
// using FastEndpoints;
// using MediatR;
//
// namespace Anonymous_Survey_Ardalis.Web.Comments;
//
// public class GetByGuid(IMediator _mediator)
//   : Endpoint<GetCommentByGuidRequest, CommentRecord>
// {
//   public override void Configure()
//   {
//     Get(GetCommentByGuidRequest.Route);
//     AllowAnonymous();
//   }
//
//   public override async Task HandleAsync(GetCommentByGuidRequest request,
//     CancellationToken cancellationToken)
//   {
//     var query = new GetCommentByGuidQuery(request.CommentGuid);
//
//     var result = await _mediator.Send(query, cancellationToken);
//
//     if (result.Status == ResultStatus.NotFound)
//     {
//       await SendNotFoundAsync(cancellationToken);
//       return;
//     }
//
//     if (result.IsSuccess)
//     {
//       Response = new CommentRecord(result.Value.CommentId, result.Value.CommentText, result.Value.SubjectId);
//       return;
//     }
//   }
// }
