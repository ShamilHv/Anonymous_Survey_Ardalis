using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Admins.Commands.Create.CreateSuperAdmin;

public class CreateSuperAdminHandler(IRepository<Admin> _repository)
  : ICommandHandler<CreateSuperAdminCommand, Result<Admin>>
{
  public async Task<Result<Admin>> Handle(CreateSuperAdminCommand request, CancellationToken cancellationToken)
  {
    var newAdmin = Admin.CreateSuperAdmin(request.AdminName, request.Email);

    await _repository.AddAsync(newAdmin, cancellationToken);
    await _repository.SaveChangesAsync(cancellationToken);

    return Result<Admin>.Success(newAdmin);
  }
}
