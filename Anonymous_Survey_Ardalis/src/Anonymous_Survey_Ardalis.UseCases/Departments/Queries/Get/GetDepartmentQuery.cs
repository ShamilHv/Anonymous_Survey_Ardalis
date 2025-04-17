using Ardalis.Result;
using MediatR;

namespace Anonymous_Survey_Ardalis.UseCases.Departments.Queries.Get;

public record GetDepartmentQuery(int Id) : IRequest<Result<DepartmentDto>>;
