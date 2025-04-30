using Anonymous_Survey_Ardalis.Core.AdminAggregate;

namespace Anonymous_Survey_Ardalis.Web.Security;


  public interface ICurrentUserService
  {
    int GetCurrentAdminId();
    string GetCurrentAdminName();
    int? GetCurrentSubjectId();
    int? GetCurrentDepartmentId();
    AdminRole GetCurrentAdminRole();
    Task<Admin?> GetCurrentAdminEntityAsync();
  }

