using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Ardalis.Result;
using MediatR;

namespace Anonymous_Survey_Ardalis.UseCases.Admins.Queries.Get;

public record GetAdminByEmailQuery(string Email) : IRequest<Result<Admin>>;
