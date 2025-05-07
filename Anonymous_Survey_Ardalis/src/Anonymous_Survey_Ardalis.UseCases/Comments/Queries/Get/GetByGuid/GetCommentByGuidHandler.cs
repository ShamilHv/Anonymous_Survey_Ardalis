using System.Security.Claims;
using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Anonymous_Survey_Ardalis.Core.CommentAggregate.Specifications;
using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.UseCases.CurrentUserServices;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Http;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Queries.Get.GetByGuid;

public class GetCommentByGuidHandler
  : IQueryHandler<GetCommentByGuidQuery, Result<CommentDto>>
{ 
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IReadRepository<Comment> _repository;


  public GetCommentByGuidHandler(
    IReadRepository<Comment> repository,
    IHttpContextAccessor httpContextAccessor)
  {
    _repository = repository;
    _httpContextAccessor = httpContextAccessor;
  }

  public async Task<Result<CommentDto>> Handle(GetCommentByGuidQuery request, CancellationToken cancellationToken)
  {
    var spec = new CommentByGuidSpec(request.CommentIdentifier);
    var comment = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

    if (comment == null)
    {
      return Result.NotFound();
    }
    
    if (!comment.IsAppropriate)
    {
      return Result.NotFound();
    }

    return new CommentDto(
      comment.Id,
      comment.SubjectId,
      comment.CommentText,
      comment.CreatedAt,
      comment.ParentCommentId,
      comment.File?.FileId,
      comment.IsAdminComment);
  }
  
}
