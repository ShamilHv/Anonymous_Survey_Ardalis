using Ardalis.Specification;

namespace Anonymous_Survey_Ardalis.Core.AdminAggregate.Specifications;

public class AdminByEmailSpec : Specification<Admin>
{
  public AdminByEmailSpec(string email)
  {
    Query
      .Where(admin => admin.Email == email);
  }
}
