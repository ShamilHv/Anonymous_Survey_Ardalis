using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Queries.Get;

public record GetCommentQuery(int commentId) : IQuery<Result<CommentDto>>;
