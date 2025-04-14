using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Subjects.Commands.Create;

public record CreateSubjectCommand(string subjectName, int departmentId) : ICommand<Result<int>>;
