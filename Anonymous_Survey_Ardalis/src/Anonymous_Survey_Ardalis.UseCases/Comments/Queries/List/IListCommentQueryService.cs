using Anonymous_Survey_Ardalis.UseCases.Admins;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Queries.List;

public interface IListCommentQueryService
{
  Task<IEnumerable<CommentDto>> ListAsync();
}
