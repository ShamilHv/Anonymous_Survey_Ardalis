using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Subjects.Queries.List;

public class ListSubjectsHandler(IListSubjectQueryService _query)
  : IQueryHandler<ListSubjectsQuery, Result<IEnumerable<SubjectDto>>>
{
  public async Task<Result<IEnumerable<SubjectDto>>> Handle(ListSubjectsQuery request,
    CancellationToken cancellationToken)
  {
    var result = await _query.ListAsync();
    return Result.Success(result);
  }
}
