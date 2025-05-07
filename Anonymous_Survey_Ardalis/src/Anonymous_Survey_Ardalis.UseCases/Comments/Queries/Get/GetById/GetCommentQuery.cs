using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Queries.Get;

public record GetCommentQuery(int CommentId, int AdminId) : IQuery<Result<CommentDto>>;
