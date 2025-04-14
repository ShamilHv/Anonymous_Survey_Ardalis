using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Subjects.Commands.Delete;

public record DeleteSubjectCommand(int subjectId) : ICommand<Result<int>>;
