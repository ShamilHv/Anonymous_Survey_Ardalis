using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Admins.Commands.Create;

public record CreateSubjectAdminCommand(string AdminName, string Email, int SubjectId, string Password)
  : ICommand<Result<Admin>>;
