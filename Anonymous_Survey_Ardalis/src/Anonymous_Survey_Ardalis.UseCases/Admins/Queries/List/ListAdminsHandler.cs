using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Admins.Queries.List;

public class ListAdminsHandler(IListAdminsQueryService _query)
  : IQueryHandler<ListAdminsQuery, Result<IEnumerable<AdminDto>>>
{
  public async Task<Result<IEnumerable<AdminDto>>> Handle(ListAdminsQuery request,
    CancellationToken cancellationToken)
  {
    var result = await _query.ListAsync();
    return Result.Success(result);
  }
}
