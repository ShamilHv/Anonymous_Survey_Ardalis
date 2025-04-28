namespace Anonymous_Survey_Ardalis.Core.Interfaces;

public interface IAdminPermissionService
{
  Task<bool> CanCommentOnSubject(int adminId, int subjectId);
  Task<bool> CanCreateAdmin(int adminId);
}
