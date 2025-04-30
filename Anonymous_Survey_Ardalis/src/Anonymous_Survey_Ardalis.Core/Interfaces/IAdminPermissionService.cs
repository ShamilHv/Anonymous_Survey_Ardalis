namespace Anonymous_Survey_Ardalis.Core.Interfaces;

public interface IAdminPermissionService
{
  Task<bool> CanCommentOnSubject(int adminId, int parenCommentId);
  Task<bool> CanCreateAdmin(int adminId);
  Task<bool> CanCreateSubject(int adminId, int departmentId);
  Task<bool> CanDeleteSubject(int adminId, int subjectId);
  Task<bool> CanModifyDepartment(int adminId);
}
