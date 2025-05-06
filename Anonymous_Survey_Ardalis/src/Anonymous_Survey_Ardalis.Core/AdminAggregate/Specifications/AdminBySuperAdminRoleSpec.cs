using Ardalis.Specification;

namespace Anonymous_Survey_Ardalis.Core.AdminAggregate.Specifications;

public class AdminBySuperAdminRoleSpec : Specification<Admin>
{
  public AdminBySuperAdminRoleSpec()
  {
    Query
      .Where(admin => admin.Role == AdminRole.SuperAdmin);
  }
}
