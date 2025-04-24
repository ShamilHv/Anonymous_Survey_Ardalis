using Ardalis.Result;
using MediatR;

namespace Anonymous_Survey_Ardalis.UseCases.Subjects.Queries;

public record GetSubjectQuery(int Id) : IRequest<Result<SubjectDto>>;
