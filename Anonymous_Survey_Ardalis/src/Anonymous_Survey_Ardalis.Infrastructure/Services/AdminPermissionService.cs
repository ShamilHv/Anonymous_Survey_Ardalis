using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.Core.AdminAggregate.Specifications;
using Anonymous_Survey_Ardalis.Core.DepartmentAggregate;
using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.Core.SubjectAggregate;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.Infrastructure.Services;

public class AdminPermissionService : IAdminPermissionService
{
  private readonly IRepository<Admin> _adminRepository;
  private readonly IRepository<Subject> _subjectRepository;
  private readonly IRepository<Department> _departmentRepository;

  public AdminPermissionService(
    IRepository<Admin> adminRepository,
    IRepository<Subject> subjectRepository,
    IRepository<Department> departmentRepository)
  {
    _adminRepository = adminRepository;
    _subjectRepository = subjectRepository;
    _departmentRepository = departmentRepository;
  }

  public async Task<bool> CanCommentOnSubject(int adminId, int subjectId)
  {
    var adminSpec = new AdminByIdSpec(adminId);
    var admin = await _adminRepository.FirstOrDefaultAsync(adminSpec);
    if (admin == null) return false;

    // SuperAdmin can comment anywhere
    if (admin.Role == AdminRole.SuperAdmin) return true;

    // SubjectAdmin can only comment on their subject
    if (admin.Role == AdminRole.SubjectAdmin)
      return admin.SubjectId == subjectId;

    // DepartmentAdmin can comment on any subject in their department
    if (admin.Role == AdminRole.DepartmentAdmin)
    {
      var subject = await _subjectRepository.GetByIdAsync(subjectId);
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
}
