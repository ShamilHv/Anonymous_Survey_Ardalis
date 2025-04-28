using System.Security.Claims;
using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Anonymous_Survey_Ardalis.Core.CommentAggregate.Specifications;
using Anonymous_Survey_Ardalis.Core.Interfaces;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Commands.Create.CreateAdminComment;

public class CreateAdminCommentHandler(
  IRepository<Comment> _repository, 
  IHttpContextAccessor httpContextAccessor,
  IAdminPermissionService _permissionService) // Add permission service
  : ICommandHandler<CreateAdminCommentCommand, Result<Comment>>
{
  public async Task<Result<Comment>> Handle(CreateAdminCommentCommand request, CancellationToken cancellationToken)
  {
    var spec = new CommentByIdSpec(request.ParentCommentId);
    var comment = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (comment == null)
    {
      return Result.NotFound("Parent Comment Not Found");
    }

    // Get the current admin ID
    var adminId = GetCurrentAdminId();
    
    // Check if the admin has permission to comment on this subject
    bool canComment = await _permissionService.CanCommentOnSubject(adminId, request.SubjectId);
    if (!canComment)
    {
      return Result.Forbidden("You don't have permission to comment on this subject");
    }

    var adminComment = new Comment(comment.SubjectId, request.CommentText)
    {
      IsAdminComment = true,
      ParentCommentId = comment.Id,
      CreatedAt = DateTime.UtcNow,
      SubjectId = request.SubjectId,
      AdminId = adminId
    };

    await _repository.AddAsync(adminComment, cancellationToken);
    await _repository.SaveChangesAsync(cancellationToken);

    return await Task.FromResult(new Result<Comment>(adminComment));
  }

  private int GetCurrentAdminId()
  {
    var httpContext = httpContextAccessor.HttpContext;
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
}
// using System.Security.Claims;
// using Anonymous_Survey_Ardalis.Core.CommentAggregate;
// using Anonymous_Survey_Ardalis.Core.CommentAggregate.Specifications;
// using Ardalis.Result;
// using Ardalis.SharedKernel;
// using Microsoft.AspNetCore.Http;
//
// namespace Anonymous_Survey_Ardalis.UseCases.Comments.Commands.Create.CreateAdminComment;
//
// public class CreateAdminCommentHandler(IRepository<Comment> _repository, IHttpContextAccessor httpContextAccessor)
//   : ICommandHandler<CreateAdminCommentCommand, Result<Comment>>
// {
//   public async Task<Result<Comment>> Handle(CreateAdminCommentCommand request, CancellationToken cancellationToken)
//   {
//     var spec = new CommentByIdSpec(request.ParentCommentId);
//     var comment = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
//     if (comment == null)
//     {
//       return Result.NotFound("Parent Comment Not Found");
//     }
//
//     var adminComment = new Comment(comment.SubjectId, request.CommentText)
//     {
//       IsAdminComment = true,
//       ParentCommentId = comment.Id,
//       CreatedAt = DateTime.UtcNow,
//       SubjectId = request.SubjectId,
//       AdminId = GetCurrentAdminId()
//     };
//
//     await _repository.AddAsync(adminComment, cancellationToken);
//     await _repository.SaveChangesAsync(cancellationToken);
//
//     return await Task.FromResult(new Result<Comment>(adminComment));
//   }
//
//   private int GetCurrentAdminId()
//   {
//     var httpContext = httpContextAccessor.HttpContext;
//     if (httpContext == null)
//     {
//       throw new InvalidOperationException("HttpContext is not available");
//     }
//
//     var adminIdString = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
//
//     if (string.IsNullOrEmpty(adminIdString))
//     {
//       throw new Exception("Admin ID claim not found in context");
//     }
//
//     if (!int.TryParse(adminIdString, out var adminId))
//     {
//       throw new Exception("Admin ID is not in valid format");
//     }
//
//     return adminId;
//   }
// }
