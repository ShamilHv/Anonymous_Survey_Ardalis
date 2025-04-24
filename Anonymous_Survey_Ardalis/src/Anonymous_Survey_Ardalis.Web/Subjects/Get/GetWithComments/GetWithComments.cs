using Anonymous_Survey_Ardalis.UseCases.Subjects;
using Anonymous_Survey_Ardalis.UseCases.Subjects.Queries.Get.GetWithComments;
using Ardalis.Result;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Subjects.Get.GetWithComments;

public class GetWithComments(IMediator _mediator)
  : Endpoint<GetSubjectWithCommentsRequest, SubjectWithCommentsDto>
{
  public override void Configure()
  {
    Get(GetSubjectWithCommentsRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(GetSubjectWithCommentsRequest request,
    CancellationToken cancellationToken)
  {
    var query = new GetSubjectWithCommentsQuery(request.SubjectId);

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
