using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Queries.Get.GetWithReplies;

public record GetCommentWithRepliesQuery(Guid CommentIdentifier) : IQuery<Result<CommentWithRepliesDto>>;
