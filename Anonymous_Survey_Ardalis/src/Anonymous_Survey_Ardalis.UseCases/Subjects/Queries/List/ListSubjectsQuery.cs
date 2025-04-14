using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Subjects.Queries.List;

public record ListSubjectsQuery : IQuery<Result<IEnumerable<SubjectDto>>>;
