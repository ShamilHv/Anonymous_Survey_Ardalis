using Anonymous_Survey_Ardalis.Core.Exceptions;
using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.UseCases.Comments.Commands.Create;
using Anonymous_Survey_Ardalis.Web.Security;
using Ardalis.Result;
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
        Response = new CreateCommentResponse(request.SubjectId, request.CommentText)
        {
          CommentId = result.Value, 
          HasFile = request.File != null
        };
      }
    }
    catch (ResourceNotFoundException ex)
    {
      ThrowError(ex.Message);
      await SendErrorsAsync(statusCode: 404, cancellation: cancellationToken);
      return;
    }
    catch (Exception ex)
    {
      ThrowError(ex.Message);
      await SendErrorsAsync(statusCode: 500, cancellation: cancellationToken);
      return;
    }
  }
}
