using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Anonymous_Survey_Ardalis.UseCases.Comments.Commands.Create.CreateAdminComment;
using Anonymous_Survey_Ardalis.UseCases.Comments.Queries.Get;
using Ardalis.SharedKernel;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Comments.Create.CreateAdminComment;

public class Create(IMediator _mediator)
  : Endpoint<CreateAdminCommentRequest, CreateAdminCommentResponse>
{
  public override void Configure()
  {
    Post(CreateAdminCommentRequest.Route);
    Summary(s =>
    {
      s.ExampleRequest = new CreateAdminCommentRequest() { CommentText = "Comment text", ParentCommentId = 2 };
    });
  }

  public override async Task HandleAsync(
    CreateAdminCommentRequest request,
    CancellationToken cancellationToken)
  {
    var query = new GetCommentQuery(request.ParentCommentId);

    var comment = await _mediator.Send(query, cancellationToken);

    if (comment == null)
    {
      throw new Exception("Comment not found");
    }
    var result = await _mediator.Send(new CreateAdminCommentCommand(request.ParentCommentId,
      request.CommentText, comment.Value.SubjectId), cancellationToken);
    
    
    if (result.IsSuccess)
    {
      Response = new CreateAdminCommentResponse(request.ParentCommentId, request.CommentText)
      {
        CommentId = result.Value.Id,
        CommentText = result.Value.CommentText,
        SubjectId = comment.Value.SubjectId
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
