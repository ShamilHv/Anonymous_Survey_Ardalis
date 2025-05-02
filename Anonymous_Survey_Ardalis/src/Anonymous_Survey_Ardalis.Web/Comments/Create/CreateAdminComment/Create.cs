using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.UseCases.Comments.Commands.Create.CreateAdminComment;
using Anonymous_Survey_Ardalis.UseCases.Comments.Queries.Get;
using Anonymous_Survey_Ardalis.UseCases.CurrentUserServices;
using Ardalis.Result;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Comments.Create.CreateAdminComment;

public class Create(
  IMediator _mediator,
  IAdminPermissionService _adminPermissionService,
  ICurrentUserService _currentUserService)
  : Endpoint<CreateAdminCommentRequest, CreateAdminCommentResponse>
{
  public override void Configure()
  {
    Post(CreateAdminCommentRequest.Route);
    AllowFormData();
    Summary(s =>
    {
      s.ExampleRequest = new CreateAdminCommentRequest { CommentText = "Comment text", ParentCommentId = 2 };
    });
  }

  public override async Task HandleAsync(
    CreateAdminCommentRequest request,
    CancellationToken cancellationToken)
  {
    var adminId = _currentUserService.GetCurrentAdminId();
    if (!await _adminPermissionService.CanCommentOnSubject(adminId, request.ParentCommentId))
    {
      await SendForbiddenAsync();
      return;
    }

    var query = new GetCommentQuery(request.ParentCommentId);

    var comment = await _mediator.Send(query, cancellationToken);

    if (comment.IsNotFound())
    {
      await SendNotFoundAsync();
      return;
    }

    var result = await _mediator.Send(new CreateAdminCommentCommand(request.ParentCommentId,
      request.CommentText, comment.Value.SubjectId), cancellationToken);


    if (result.IsSuccess)
    {
      Response = new CreateAdminCommentResponse(request.ParentCommentId, request.CommentText)
      {
        CommentId = result.Value.Id, CommentText = result.Value.CommentText, SubjectId = comment.Value.SubjectId
      };
    }
  }
}
// {
// public int CommentId { get; set; }
// public int SubjectId { get; set; } 
// public string CommentText { get; set; } = commentText;
// public int? ParentCommentId { get; set; } = parentCommentId;
// public bool HasFile { get; set; } = false;
// public string? FilePath { get; set; }
// public bool IsAdminComment { get; set; } = true;
// }

// await _repository.AddAsync(adminComment, cancellationToken);
// await _repository.SaveChangesAsync(cancellationToken);
