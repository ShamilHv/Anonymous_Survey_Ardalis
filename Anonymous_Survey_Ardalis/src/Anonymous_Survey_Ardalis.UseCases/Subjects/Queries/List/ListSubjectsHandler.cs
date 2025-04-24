using Anonymous_Survey_Ardalis.Core.SubjectAggregate;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Subjects.Queries.List;

public class ListSubjectsHandler(IRepository<Subject> repository)
  : IQueryHandler<ListSubjectsQuery, Result<IEnumerable<SubjectDto>>>
{
  public async Task<Result<IEnumerable<SubjectDto>>> Handle(ListSubjectsQuery request,
    CancellationToken cancellationToken)
  {
    var subjects = await repository.ListAsync(cancellationToken);
    var dtoList = subjects.Select(s => new SubjectDto(
      s.Id,
      s.SubjectName,
      s.DepartmentId,
      s.CreatedAt
    ));
    return Result.Success(dtoList);
  }
}
