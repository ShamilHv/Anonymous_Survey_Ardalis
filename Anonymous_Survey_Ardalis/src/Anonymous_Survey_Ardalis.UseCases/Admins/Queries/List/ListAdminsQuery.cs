using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Admins.Queries.List;

public record ListAdminsQuery : IQuery<Result<IEnumerable<AdminDto>>>;
