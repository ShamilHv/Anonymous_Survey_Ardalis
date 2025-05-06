using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Commands.UpdateSubject;

public record UpdateCommentSubjectCommand(int CommentId, int NewSubjectId, int AdminId) : ICommand<Result<bool>>;
