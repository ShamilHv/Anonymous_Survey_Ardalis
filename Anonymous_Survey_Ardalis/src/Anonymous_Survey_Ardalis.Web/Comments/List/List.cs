using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.UseCases.Comments.Queries.List;
using Anonymous_Survey_Ardalis.Web.Security;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Comments;

public class List(IMediator _mediator) : Endpoint<CommentListRequest, CommentListResponse>
{
  public override void Configure()
  {
    Get("/Comments");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CommentListRequest request, CancellationToken cancellationToken)
  {
    
    var query = new ListCommentsQuery
    {
      PageNumber = request.PageNumber,
      PageSize = request.PageSize,
      SubjectId = request.SubjectId
    };
    
    var result = await _mediator.Send(query, cancellationToken);

    if (result.IsSuccess)
    {
      Response = new CommentListResponse
      {
        Comments = result.Value.Items.Select(c => 
          new CommentRecord(c.CommentId, c.CommentText, c.SubjectId)).ToList(),
        PageNumber = result.Value.PageNumber,
        PageSize = result.Value.PageSize,
        TotalPages = result.Value.TotalPages,
        TotalCount = result.Value.TotalCount
      };
    }
    else
    {
      ThrowError(result.Errors.FirstOrDefault() ?? "Failed to retrieve comments");
    }
  }
}
// using Anonymous_Survey_Ardalis.UseCases.Comments.Queries.List;
// using FastEndpoints;
// using MediatR;
//
// namespace Anonymous_Survey_Ardalis.Web.Comments;
//
// public class List(IMediator _mediator) : EndpointWithoutRequest<CommentListResponse>
// {
//   public override void Configure()
//   {
//     Get("/Comments");
//     AllowAnonymous();
//   }
//
//   public override async Task HandleAsync(CancellationToken cancellationToken)
//   {
//     var result =
//       await _mediator.Send(new ListCommentsQuery(), cancellationToken);
//
//     if (result.IsSuccess)
//     {
//       Response = new CommentListResponse
//       {
//         Comments = result.Value.Select(c => new CommentRecord(c.CommentId, c.CommentText, c.SubjectId)).ToList()
//       };
//     }
//   }
// }
