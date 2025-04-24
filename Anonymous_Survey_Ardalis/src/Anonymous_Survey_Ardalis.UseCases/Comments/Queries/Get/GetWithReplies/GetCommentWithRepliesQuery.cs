using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Queries.Get.GetWithReplies;

public record GetCommentWithRepliesQuery(int CommentId) : IQuery<Result<CommentWithRepliesDto>>;
