using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Admins.Commands.Create.CreateSuperAdmin;

public record CreateSuperAdminCommand(string AdminName, string Email, string Password)
  : ICommand<Result<Admin>>;
