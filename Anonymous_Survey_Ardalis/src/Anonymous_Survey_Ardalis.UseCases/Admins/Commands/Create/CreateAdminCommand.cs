using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Admins.Commands.Create;

public record CreateAdminCommand(string AdminName, string Email, int SubjectId) : ICommand<Result<int>>;
