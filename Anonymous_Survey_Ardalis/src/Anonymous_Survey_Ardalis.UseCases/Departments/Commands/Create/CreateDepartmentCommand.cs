using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Departments.Commands.Create;

public record CreateDepartmentCommand(string DepartmentName) : ICommand<Result<int>>;
