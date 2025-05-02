using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Queries.Get.GetByGuid;

public record GetCommentByGuidQuery(Guid CommentIdentifier) : IQuery<Result<CommentDto>>;
