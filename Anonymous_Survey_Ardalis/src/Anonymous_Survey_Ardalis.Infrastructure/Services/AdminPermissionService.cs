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
    private readonly IRepository<Subject> _subjectRepository;
    private readonly IRepository<Department> _departmentRepository;
    private readonly IRepository<Comment> _commentRepository;

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
        if (admin == null) throw new Exception("Can't find admin");
        var commentSpec = new CommentByIdSpec(parentCommentId);
        var comment = await _commentRepository.FirstOrDefaultAsync(commentSpec);
        if(comment == null) throw new Exception("Can't find comment");
        
        // SuperAdmin can comment anywhere
        if (admin.Role == AdminRole.SuperAdmin) return true;
        
        // SubjectAdmin can only comment on their subject
        if (admin.Role == AdminRole.SubjectAdmin)
            return admin.SubjectId == comment.SubjectId;
        
        // DepartmentAdmin can comment on any subject in their department
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
        if (admin == null) return false;
        
        if (admin.Role == AdminRole.SuperAdmin) return true;
        
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
        if (admin == null) return false;
        
        if (admin.Role == AdminRole.SuperAdmin) return true;
        
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
        if (admin == null) return false;
        
        if (admin.Role == AdminRole.SuperAdmin)
        {
            return true;
        }
        
        return false;
    }
}
// using Anonymous_Survey_Ardalis.Core.AdminAggregate;
// using Anonymous_Survey_Ardalis.Core.AdminAggregate.Specifications;
// using Anonymous_Survey_Ardalis.Core.DepartmentAggregate;
// using Anonymous_Survey_Ardalis.Core.Interfaces;
// using Anonymous_Survey_Ardalis.Core.SubjectAggregate;
// using Ardalis.SharedKernel;
//
// namespace Anonymous_Survey_Ardalis.Infrastructure.Services;
//
// public class AdminPermissionService : IAdminPermissionService
// {
//   private readonly IRepository<Admin> _adminRepository;
//   private readonly IRepository<Subject> _subjectRepository;
//   private readonly IRepository<Department> _departmentRepository;
//
//   public AdminPermissionService(
//     IRepository<Admin> adminRepository,
//     IRepository<Subject> subjectRepository,
//     IRepository<Department> departmentRepository)
//   {
//     _adminRepository = adminRepository;
//     _subjectRepository = subjectRepository;
//     _departmentRepository = departmentRepository;
//   }
//
//   public async Task<bool> CanCommentOnSubject(int adminId, int parentCommentId)
//   {
//     var adminSpec = new AdminByIdSpec(adminId);
//     var admin = await _adminRepository.FirstOrDefaultAsync(adminSpec);
//     if (admin == null) return false;
//
//     // SuperAdmin can comment anywhere
//     if (admin.Role == AdminRole.SuperAdmin) return true;
//
//     // SubjectAdmin can only comment on their subject
//     if (admin.Role == AdminRole.SubjectAdmin)
//       return admin.SubjectId == parentCommentId;
//
//     // DepartmentAdmin can comment on any subject in their department
//     if (admin.Role == AdminRole.DepartmentAdmin)
//     {
//       var subject = await _subjectRepository.GetByIdAsync(parentCommentId);
//       return subject?.DepartmentId == admin.DepartmentId;
//     }
//
//     return false;
//   }
//
//   public async Task<bool> CanCreateAdmin(int adminId)
//   {
//     var adminSpec = new AdminByIdSpec(adminId);
//     var admin = await _adminRepository.FirstOrDefaultAsync(adminSpec);
//     
//     
//     return admin?.Role == AdminRole.SuperAdmin;
//   }
//
//   public async Task<bool> CanModifySubject(int adminId, int parentCommentId)
//   {
//     var adminSpec = new AdminByIdSpec(adminId);
//     var admin = await _adminRepository.FirstOrDefaultAsync(adminSpec);
//     if (admin == null) return false;
//     
//     if (admin.Role == AdminRole.SuperAdmin) return true;
//
//     if (admin.Role == AdminRole.DepartmentAdmin)
//     {     
//       var subject = await _subjectRepository.GetByIdAsync(parentCommentId);
//       return subject?.DepartmentId == admin.DepartmentId;
//     }
//   return false;
//     
//   }
//
//   public async Task<bool> CanModifyDepartment(int adminId, int departmentId)
//   {
//     var adminSpec = new AdminByIdSpec(adminId);
//     var admin = await _adminRepository.FirstOrDefaultAsync(adminSpec);
//     if (admin == null) return false;
//
//     if (admin.Role == AdminRole.SuperAdmin)
//     {
//       return true;
//     }
//     return false;
//   }
// }
