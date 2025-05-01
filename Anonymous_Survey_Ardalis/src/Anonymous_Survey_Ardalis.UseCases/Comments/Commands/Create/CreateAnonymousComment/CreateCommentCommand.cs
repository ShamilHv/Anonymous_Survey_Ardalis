using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Http;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Commands.Create;

public record CreateCommentCommand(int SubjectId, string CommentText, IFormFile? File) : ICommand<Result<Guid>>;
