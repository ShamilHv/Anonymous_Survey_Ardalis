using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.Core.AdminAggregate.Specifications;
using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Anonymous_Survey_Ardalis.Core.CommentAggregate.Specifications;
using Anonymous_Survey_Ardalis.Core.DepartmentAggregate;
using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.Core.SubjectAggregate;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.Infrastructure.Services;

public class AdminPermissionService : IAdminPermissionService
{
  private readonly IRepository<Admin> _adminRepository;
  private readonly IRepository<Comment> _commentRepository;
  private readonly IRepository<Department> _departmentRepository;
  private readonly IRepository<Subject> _subjectRepository;

  public AdminPermissionService(
    IRepository<Admin> adminRepository,
    IRepository<Subject> subjectRepository,
    IRepository<Department> departmentRepository,
    IRepository<Comment> commentRepository)
  {
    _adminRepository = adminRepository;
    _subjectRepository = subjectRepository;
    _departmentRepository = departmentRepository;
    _commentRepository = commentRepository;
  }

  public async Task<bool> CanCommentOnSubject(int adminId, int parentCommentId)
  {
    var adminSpec = new AdminByIdSpec(adminId);
    var admin = await _adminRepository.FirstOrDefaultAsync(adminSpec);
    if (admin == null)
    {
      throw new Exception("Can't find admin");
    }

    var commentSpec = new CommentByIdSpec(parentCommentId);
    var comment = await _commentRepository.FirstOrDefaultAsync(commentSpec);
    if (comment == null)
    {
      throw new Exception("Can't find comment");
    }

    // SuperAdmin can comment anywhere
    if (admin.Role == AdminRole.SuperAdmin)
    {
      return true;
    }

    // SubjectAdmin can only comment on their subject
    if (admin.Role == AdminRole.SubjectAdmin)
    {
      return admin.SubjectId == comment.SubjectId;
    }

    // DepartmentAdmin can comment on any subject in their department
    if (admin.Role == AdminRole.DepartmentAdmin)
    {
      var subject = await _subjectRepository.GetByIdAsync(comment.SubjectId);
      return subject?.DepartmentId == admin.DepartmentId;
    }

    return false;
  }


  public async Task<bool> CanGetComments(int adminId, int? departmentId, int? subjectId)
  {
    var adminSpec = new AdminByIdSpec(adminId);
    var admin = await _adminRepository.FirstOrDefaultAsync(adminSpec);
    if (admin == null)
    {
      throw new Exception("Can't find admin");
    }

    // SuperAdmin can view any comments, with or without filters
    if (admin.Role == AdminRole.SuperAdmin)
    {
      // If both departmentId and subjectId are provided, verify they match
      if (departmentId.HasValue && subjectId.HasValue)
      {
        var subject = await _subjectRepository.GetByIdAsync(subjectId.Value);
        if (subject == null)
        {
          throw new Exception("Can't find subject");
        }

        if (subject.DepartmentId != departmentId.Value)
        {
          throw new Exception("Subject does not belong to the specified department");
        }
      }

      return true;
    }

    // Subject admin can only access their own subject's comments
    if (admin.Role == AdminRole.SubjectAdmin)
    {
      // If no subject specified or matches admin's subject, allow access
      if (!subjectId.HasValue || subjectId.Value == admin.SubjectId)
      {
        return true;
      }

      // Otherwise, deny access - subject admin can only see their subject
      return false;
    }

    // Department admin can access any subject in their department
    if (admin.Role == AdminRole.DepartmentAdmin)
    {
      // If department specified, must match admin's department
      if (departmentId.HasValue && departmentId.Value != admin.DepartmentId)
      {
        return false;
      }

      // If subject specified, must belong to admin's department
      if (subjectId.HasValue)
      {
        var subject = await _subjectRepository.GetByIdAsync(subjectId.Value);
        if (subject == null)
        {
          throw new Exception("Can't find subject");
        }

        return subject.DepartmentId == admin.DepartmentId;
      }

      // No specific filters, allow access to department's subjects
      return true;
    }

    return false;
  }

  public async Task<bool> CanDownloadFile(int adminId, int fileId)
  {
    var adminSpec = new AdminByIdSpec(adminId);
    var admin = await _adminRepository.FirstOrDefaultAsync(adminSpec);
    if (admin == null)
    {
      return false;
    }

    // SuperAdmin can download any file
    if (admin.Role == AdminRole.SuperAdmin)
    {
      return true;
    }

    // Get the comment that contains this file
    var comments = await _commentRepository.ListAsync(
      new CommentByFileIdSpec(fileId));
  
    var comment = comments.FirstOrDefault();
    if (comment == null)
    {
      return false; // File not found in any comment
    }

    // SubjectAdmin can only download files from their subject
    if (admin.Role == AdminRole.SubjectAdmin)
    {
      return admin.SubjectId == comment.SubjectId;
    }

    // DepartmentAdmin can download files from any subject in their department
    if (admin.Role == AdminRole.DepartmentAdmin)
    {
      var subject = await _subjectRepository.GetByIdAsync(comment.SubjectId);
      return subject?.DepartmentId == admin.DepartmentId;
    }

    return false;
  }
  public async Task<bool> CanCreateAdmin(int adminId)
  {
    var adminSpec = new AdminByIdSpec(adminId);
    var admin = await _adminRepository.FirstOrDefaultAsync(adminSpec);
    return admin?.Role == AdminRole.SuperAdmin;
  }

  public async Task<bool> CanCreateSubject(int adminId, int departmentId)
  {
    var adminSpec = new AdminByIdSpec(adminId);
    var admin = await _adminRepository.FirstOrDefaultAsync(adminSpec);
    if (admin == null)
    {
      return false;
    }

    if (admin.Role == AdminRole.SuperAdmin)
    {
      return true;
    }

    if (admin.Role == AdminRole.DepartmentAdmin)
    {
      var department = await _departmentRepository.GetByIdAsync(departmentId);
      if (department is null)
      {
        return false;
      }

      return department.Id == admin.DepartmentId;
    }

    return false;
  }

  public async Task<bool> CanDeleteSubject(int adminId, int subjectId)
  {
    var adminSpec = new AdminByIdSpec(adminId);
    var admin = await _adminRepository.FirstOrDefaultAsync(adminSpec);
    if (admin == null)
    {
      return false;
    }

    if (admin.Role == AdminRole.SuperAdmin)
    {
      return true;
    }

    if (admin.Role == AdminRole.DepartmentAdmin)
    {
      var subject = await _subjectRepository.GetByIdAsync(subjectId);
      if (subject is null)
      {
        return false;
      }

      return subject.DepartmentId == admin.DepartmentId;
    }

    return false;
  }

  public async Task<bool> CanModifyDepartment(int adminId)
  {
    var adminSpec = new AdminByIdSpec(adminId);
    var admin = await _adminRepository.FirstOrDefaultAsync(adminSpec);
    if (admin == null)
    {
      return false;
    }

    if (admin.Role == AdminRole.SuperAdmin)
    {
      return true;
    }

    return false;
  }
}
