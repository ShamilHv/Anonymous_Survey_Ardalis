using Anonymous_Survey_Ardalis.UseCases.Comments.Commands.Create;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Comments;

public class Create(IMediator _mediator)
  : Endpoint<CreateCommentRequest, CreateCommentResponse>
{
  public override void Configure()
  {
    Post(CreateCommentRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.ExampleRequest = new CreateCommentRequest { SubjectId = 1 };
    });
  }

  public override async Task HandleAsync(
    CreateCommentRequest request,
    CancellationToken cancellationToken)
  {
    var result = await _mediator.Send(new CreateCommentCommand(request.SubjectId,
      request.CommentText, request.File), cancellationToken);

    if (result.IsSuccess)
    {
      Response = new CreateCommentResponse(request.SubjectId, request.CommentText) { CommentId = result.Value };
    }
  }
}
