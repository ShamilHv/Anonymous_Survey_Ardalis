using Anonymous_Survey_Ardalis.UseCases.Admins;
using Anonymous_Survey_Ardalis.UseCases.Admins.Queries.List;

namespace Anonymous_Survey_Ardalis.Infrastructure.Data.Queries;

public class ListAdminsQueryService : IListAdminsQueryService
{
  public Task<IEnumerable<AdminDto>> ListAsync()
  {
    List<AdminDto> fakeComments =
    [
      new(1, "sd", "klsdm", 1, DateTime.UtcNow.AddYears(-1)),
      new(2, "sd", "klsdm", 1, DateTime.UtcNow.AddYears(+1))
    ];

    return Task.FromResult(fakeComments.AsEnumerable());
  }
}
