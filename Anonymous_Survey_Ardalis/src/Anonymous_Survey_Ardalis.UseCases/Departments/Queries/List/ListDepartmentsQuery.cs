using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Departments.Queries.List;

public record ListDepartmentsQuery : IQuery<Result<IEnumerable<DepartmentDto>>>;
