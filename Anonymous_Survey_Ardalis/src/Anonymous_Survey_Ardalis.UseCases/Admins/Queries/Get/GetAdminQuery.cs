using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Ardalis.Result;
using MediatR;

namespace Anonymous_Survey_Ardalis.UseCases.Admins.Queries.Get;

public record GetAdminQuery(int Id) : IRequest<Result<Admin>>;
