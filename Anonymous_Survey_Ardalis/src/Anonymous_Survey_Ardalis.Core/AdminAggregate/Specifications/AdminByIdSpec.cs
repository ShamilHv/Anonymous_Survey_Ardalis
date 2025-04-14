using Ardalis.Specification;

namespace Anonymous_Survey_Ardalis.Core.AdminAggregate.Specifications;

public class AdminByIdSpec : Specification<Admin>
{
  public AdminByIdSpec(int adminId)
  {
    Query
      .Where(admin => admin.Id == adminId);
  }
}
