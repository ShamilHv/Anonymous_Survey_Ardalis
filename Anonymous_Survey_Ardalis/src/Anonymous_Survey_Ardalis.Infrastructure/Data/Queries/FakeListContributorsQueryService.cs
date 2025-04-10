using Anonymous_Survey_Ardalis.UseCases.Contributors;
using Anonymous_Survey_Ardalis.UseCases.Contributors.List;

namespace Anonymous_Survey_Ardalis.Infrastructure.Data.Queries;

public class FakeListContributorsQueryService : IListContributorsQueryService
{
  public Task<IEnumerable<ContributorDTO>> ListAsync()
  {
    List<ContributorDTO> result =
    [
      new(1, "Fake Contributor 1", ""),
      new(2, "Fake Contributor 2", "")
    ];

    return Task.FromResult(result.AsEnumerable());
  }
}
