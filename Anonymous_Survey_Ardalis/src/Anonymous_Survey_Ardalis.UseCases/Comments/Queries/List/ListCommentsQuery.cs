using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Queries.List;

public record ListCommentsQuery : IQuery<Result<IEnumerable<CommentDto>>>;
