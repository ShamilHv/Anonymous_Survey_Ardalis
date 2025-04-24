using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Commands.Create.CreateAdminComment;

public record CreateAdminCommentCommand(int ParentCommentId, string CommentText, int SubjectId)
  : ICommand<Result<Comment>>;
