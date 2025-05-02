using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.Core.SubjectAggregate;
using Anonymous_Survey_Ardalis.Core.SubjectAggregate.Specifications;
using Anonymous_Survey_Ardalis.UseCases.Common;
using Anonymous_Survey_Ardalis.UseCases.CurrentUserServices;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Queries.List;

public class ListCommentsHandler : IQueryHandler<ListCommentsQuery, Result<PagedResponse<CommentDto>>>
{
  private readonly IReadRepository<Comment> _commentRepository;
  private readonly ICurrentUserService _currentUserService;
  private readonly IAdminPermissionService _permissionService;
  private readonly IReadRepository<Subject> _subjectRepository;

  public ListCommentsHandler(
    IReadRepository<Comment> commentRepository,
    IReadRepository<Subject> subjectRepository,
    IAdminPermissionService permissionService,
    ICurrentUserService currentUserService)
  {
    _commentRepository = commentRepository;
    _subjectRepository = subjectRepository;
    _permissionService = permissionService;
    _currentUserService = currentUserService;
  }

  public async Task<Result<PagedResponse<CommentDto>>> Handle(ListCommentsQuery request,
    CancellationToken cancellationToken)
  {
    try
    {
      var adminId = _currentUserService.GetCurrentAdminId();
      var adminRole = _currentUserService.GetCurrentAdminRole();
      var adminSubjectId = _currentUserService.GetCurrentSubjectId();
      var adminDepartmentId = _currentUserService.GetCurrentDepartmentId();

      // Initialize variables for effective filtering
      int? effectiveSubjectId = null;
      List<int>? subjectIds = null;

      switch (adminRole)
      {
        case AdminRole.SuperAdmin:
          effectiveSubjectId = request.SubjectId;
          
          // If department filter is provided, get all subjects in that department
          if (request.DepartmentId.HasValue && !request.SubjectId.HasValue)
          {
            var subjectSpec = new SubjectsByDepartmentSpec(request.DepartmentId.Value);
            var departmentSubjects = await _subjectRepository.ListAsync(subjectSpec, cancellationToken);
            subjectIds = departmentSubjects.Select(s => s.Id).ToList();
          }
          break;

        case AdminRole.SubjectAdmin:
          // Subject admin can ONLY see their assigned subject, regardless of what was requested
          if (!adminSubjectId.HasValue)
          {
            return Result.Error("Subject admin doesn't have an assigned subject");
          }
          
          effectiveSubjectId = adminSubjectId.Value;
          break;

        case AdminRole.DepartmentAdmin:
          if (!adminDepartmentId.HasValue)
          {
            return Result.Error("Department admin doesn't have an assigned department");
          }

          if (request.SubjectId.HasValue)
          {
            var subject = await _subjectRepository.GetByIdAsync(request.SubjectId.Value, cancellationToken);
            if (subject == null)
            {
              return Result.Error("Subject not found");
            }
            
            if (subject.DepartmentId != adminDepartmentId.Value)
            {
              return Result.Error("Subject does not belong to this admin's department");
            }
            
            effectiveSubjectId = request.SubjectId.Value;
          }
          else
          {
            var subjectSpec = new SubjectsByDepartmentSpec(adminDepartmentId.Value);
            var departmentSubjects = await _subjectRepository.ListAsync(subjectSpec, cancellationToken);
            subjectIds = departmentSubjects.Select(s => s.Id).ToList();
            
            if (!subjectIds.Any())
            {
              return Result.Success(new PagedResponse<CommentDto>
              {
                Items = new List<CommentDto>(),
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = 0,
                TotalPages = 0
              });
            }
          }
          break;
          
        default:
          return Result.Error("Unknown admin role");
      }

      var specification = new CommentPaginatedSpecification(
        request.PageNumber,
        request.PageSize,
        effectiveSubjectId,
        null, 
        subjectIds);

      var totalCount = await _commentRepository.CountAsync(specification, cancellationToken);
      var comments = await _commentRepository.ListAsync(specification, cancellationToken);

      var commentDtos = comments.Select(c =>
        new CommentDto(c.Id, c.SubjectId, c.CommentText, c.CreatedAt, c.ParentCommentId, c.FileId, c.IsAdminComment));

      var pagedResponse = new PagedResponse<CommentDto>
      {
        Items = commentDtos.ToList(),
        PageNumber = request.PageNumber,
        PageSize = request.PageSize,
        TotalCount = totalCount,
        TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
      };

      return Result.Success(pagedResponse);
    }
    catch (Exception ex)
    {
      return Result.Error(ex.Message);
    }
  }
}
