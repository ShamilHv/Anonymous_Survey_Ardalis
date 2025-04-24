using Ardalis.Result;
using MediatR;

namespace Anonymous_Survey_Ardalis.UseCases.Departments.Queries.GetWithSubjects;

public record GetDepartmentWithSubjectsQuery(int Id) : IRequest<Result<DepartmentWithSubjectsDto>>;
