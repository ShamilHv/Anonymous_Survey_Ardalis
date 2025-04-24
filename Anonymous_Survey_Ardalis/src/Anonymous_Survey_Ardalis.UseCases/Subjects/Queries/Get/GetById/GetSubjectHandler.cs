using Anonymous_Survey_Ardalis.Core.SubjectAggregate;
using Anonymous_Survey_Ardalis.Core.SubjectAggregate.Specifications;
using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;

namespace Anonymous_Survey_Ardalis.UseCases.Subjects.Queries;

public class GetSubjectHandler(IReadRepository<Subject> repository)
  : IRequestHandler<GetSubjectQuery, Result<SubjectDto>>
{
  public async Task<Result<SubjectDto>> Handle(GetSubjectQuery request, CancellationToken cancellationToken)
  {
    var spec = new SubjectByIdSpec(request.Id);
    var subject = await repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (subject == null)
    {
      return Result.NotFound();
    }

    return new SubjectDto(subject.Id, subject.SubjectName, subject.DepartmentId, DateTime.UtcNow);
  }
}
