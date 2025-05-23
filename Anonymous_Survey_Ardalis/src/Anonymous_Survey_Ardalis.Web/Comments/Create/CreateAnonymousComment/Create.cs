using Anonymous_Survey_Ardalis.Core.Exceptions;
using Anonymous_Survey_Ardalis.UseCases.Comments.Commands.Create;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Comments.Create.CreateAnonymousComment;

public class Create(IMediator _mediator)
  : Endpoint<CreateCommentRequest, CreateCommentResponse>
{
  public override void Configure()
  {
    Post(CreateCommentRequest.Route);
    AllowAnonymous();
    AllowFormData();
    Summary(s =>
    {
      s.ExampleRequest = new CreateCommentRequest { SubjectId = 1 };
    });
  }

  public override async Task HandleAsync(
    CreateCommentRequest request,
    CancellationToken cancellationToken)
  {
    try
    {
      var result = await _mediator.Send(new CreateCommentCommand(request.SubjectId,
        request.CommentText, request.File), cancellationToken);

      if (result.IsSuccess)
      {
        Response = new CreateCommentResponse("https://localhost:57679/Comments/" + result.Value);
      }
    }
    catch (ResourceNotFoundException ex)
    {
      ThrowError(ex.Message);
      await SendErrorsAsync(404, cancellationToken);
    }
    catch (Exception ex)
    {
      ThrowError(ex.Message);
      await SendErrorsAsync(500, cancellationToken);
    }
  }
}
