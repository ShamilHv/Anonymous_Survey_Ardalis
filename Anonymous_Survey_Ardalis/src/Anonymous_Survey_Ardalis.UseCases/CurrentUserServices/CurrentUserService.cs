using System.Security.Claims;
using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.UseCases.Admins.Queries.Get;
using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Anonymous_Survey_Ardalis.UseCases.CurrentUserServices;

public class CurrentUserService : ICurrentUserService
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IMediator _mediator;

  public CurrentUserService(
    IHttpContextAccessor httpContextAccessor,
    IMediator mediator)
  {
    _httpContextAccessor = httpContextAccessor;
    _mediator = mediator;
  }

  public int GetCurrentAdminId()
  {
    var httpContext = _httpContextAccessor.HttpContext;
    if (httpContext == null)
    {
      throw new InvalidOperationException("HttpContext is not available");
    }

    var adminIdString = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

    if (string.IsNullOrEmpty(adminIdString))
    {
      throw new Exception("Admin ID claim not found in context");
    }

    if (!int.TryParse(adminIdString, out var adminId))
    {
      throw new Exception("Admin ID is not in valid format");
    }

    return adminId;
  }

  public string GetCurrentAdminName()
  {
    var httpContext = _httpContextAccessor.HttpContext;
    if (httpContext == null)
    {
      throw new InvalidOperationException("HttpContext is not available");
    }

    var adminName = httpContext.User.FindFirstValue(ClaimTypes.Name);

    if (string.IsNullOrEmpty(adminName))
    {
      throw new Exception("Admin name claim not found in context");
    }

    return adminName;
  }

  public int? GetCurrentSubjectId()
  {
    var httpContext = _httpContextAccessor.HttpContext;
    if (httpContext == null)
    {
      return null;
    }

    var subjectIdString = httpContext.User.FindFirstValue("SubjectId");

    if (string.IsNullOrEmpty(subjectIdString) || subjectIdString == "0")
    {
      return null;
    }

    if (int.TryParse(subjectIdString, out var subjectId))
    {
      return subjectId;
    }

    return null;
  }

  public int? GetCurrentDepartmentId()
  {
    var httpContext = _httpContextAccessor.HttpContext;
    if (httpContext == null)
    {
      return null;
    }

    var departmentIdString = httpContext.User.FindFirstValue("DepartmentId");

    if (string.IsNullOrEmpty(departmentIdString) || departmentIdString == "0")
    {
      return null;
    }

    if (int.TryParse(departmentIdString, out var departmentId))
    {
      return departmentId;
    }

    return null;
  }

  public AdminRole GetCurrentAdminRole()
  {
    var httpContext = _httpContextAccessor.HttpContext;
    if (httpContext == null)
    {
      throw new InvalidOperationException("HttpContext is not available");
    }

    var roleString = httpContext.User.FindFirstValue(ClaimTypes.Role);

    if (string.IsNullOrEmpty(roleString))
    {
      throw new Exception("Admin role claim not found in context");
    }

    if (Enum.TryParse<AdminRole>(roleString, out var role))
    {
      return role;
    }

    throw new Exception("Admin role is not in valid format");
  }

  public async Task<Admin?> GetCurrentAdminEntityAsync()
  {
    var adminId = GetCurrentAdminId();
    var result = await _mediator.Send(new GetAdminQuery(adminId));

    if (result.Status != ResultStatus.Ok || result.Value == null)
    {
      throw new Exception("Admin could not be found");
    }

    return result.Value;
  }

  public HttpContext? GetHttpContext()
  {
    return _httpContextAccessor.HttpContext;
  }
}
