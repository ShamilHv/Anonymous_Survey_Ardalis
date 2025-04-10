using Ardalis.Specification;

namespace Anonymous_Survey_Ardalis.Core.ContributorAggregate.Specifications;

public class ContributorByIdSpec : Specification<Contributor>
{
  public ContributorByIdSpec(int contributorId)
  {
    Query
      .Where(contributor => contributor.Id == contributorId);
  }
}
