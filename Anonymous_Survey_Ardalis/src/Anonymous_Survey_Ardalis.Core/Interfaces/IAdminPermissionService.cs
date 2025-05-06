namespace Anonymous_Survey_Ardalis.Core.Interfaces;

public interface IAdminPermissionService
{
  Task<bool> CanCommentOnSubject(int adminId, int parenCommentId);
  Task<bool> CanCreateAdmin(int adminId);
  Task<bool> CanCreateSubject(int adminId, int departmentId);
  Task<bool> CanDeleteSubject(int adminId, int subjectId);
  Task<bool> CanModifyDepartment(int adminId);
  Task<bool> CanGetComments(int adminId, int? departmentId, int? subjectId);
  Task<bool> CanDownloadFile(int adminId, int fileId);
  Task<bool> CanRequestSubjectChange(int adminId, int commentId);
  public Task<bool> CanUpdateCommentSubject(int adminId);

}
