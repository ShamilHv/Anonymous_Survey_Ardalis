using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Admins.Commands.Create.CreateDepartmentAdmin;

public record CreateDepartmentAdminCommand(string AdminName, string Email, int DepartmentId, string Password)
  : ICommand<Result<Admin>>;
