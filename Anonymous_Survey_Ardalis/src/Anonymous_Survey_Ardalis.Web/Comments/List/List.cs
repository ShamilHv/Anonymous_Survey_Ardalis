using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.Core.SubjectAggregate;
using Anonymous_Survey_Ardalis.UseCases.Comments.Queries.List;
using Anonymous_Survey_Ardalis.UseCases.CurrentUserServices;
using Ardalis.SharedKernel;
using FastEndpoints;
using MediatR;

namespace Anonymous_Survey_Ardalis.Web.Comments;

public class List : Endpoint<CommentListRequest, CommentListResponse>
{
  private readonly ICurrentUserService _currentUserService;
  private readonly IMediator _mediator;
  private readonly IAdminPermissionService _permissionService;
  private readonly IReadRepository<Subject> _subjectRepository;

  public List(
    IMediator mediator,
    ICurrentUserService currentUserService,
    IAdminPermissionService permissionService,
    IReadRepository<Subject> subjectRepository)
  {
    _mediator = mediator;
    _currentUserService = currentUserService;
    _permissionService = permissionService;
    _subjectRepository = subjectRepository;
  }

  public override void Configure()
  {
    Get("/Comments");
  }

  public override async Task HandleAsync(CommentListRequest request, CancellationToken cancellationToken)
  {
    try
    {
      var adminId = _currentUserService.GetCurrentAdminId();
      var adminRole = _currentUserService.GetCurrentAdminRole();

      // Super admin specific validations
      if (adminRole == AdminRole.SuperAdmin && request.SubjectId.HasValue && request.DepartmentId.HasValue)
      {
        var subject = await _subjectRepository.GetByIdAsync(request.SubjectId.Value, cancellationToken);
        if (subject == null)
        {
          ThrowError("Subject not found");
          return;
        }

        if (subject.DepartmentId != request.DepartmentId.Value)
        {
          ThrowError("Subject does not belong to the specified department");
          return;
        }
      }

      var query = new ListCommentsQuery
      {
        PageNumber = request.PageNumber,
        PageSize = request.PageSize,
        SubjectId = request.SubjectId,
        DepartmentId = request.DepartmentId
      };

      var result = await _mediator.Send(query, cancellationToken);

      if (result.IsSuccess)
      {
        Response = new CommentListResponse
        {
          Comments = result.Value.Items.Select(c =>
            new CommentRecord(c.CommentId, c.CommentText, c.SubjectId)).ToList(),
          PageNumber = result.Value.PageNumber,
          PageSize = result.Value.PageSize,
          TotalPages = result.Value.TotalPages,
          TotalCount = result.Value.TotalCount
        };
      }
      else
      {
        ThrowError(result.Errors.FirstOrDefault() ?? "Failed to retrieve comments");
      }
    }
    catch (Exception ex)
    {
      ThrowError(ex.Message);
    }
  }
}
