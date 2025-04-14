using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Departments.Commands.Delete;

public record DeleteDepartmentCommand(int departmentId) : ICommand<Result<int>>;
