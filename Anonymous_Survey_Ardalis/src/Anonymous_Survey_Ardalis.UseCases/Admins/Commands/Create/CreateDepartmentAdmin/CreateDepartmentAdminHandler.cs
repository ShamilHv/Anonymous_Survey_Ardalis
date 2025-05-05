using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Admins.Commands.Create.CreateDepartmentAdmin;

public class CreateDepartmentAdminHandler(IRepository<Admin> _repository)
  : ICommandHandler<CreateDepartmentAdminCommand, Result<Admin>>
{
  public async Task<Result<Admin>> Handle(CreateDepartmentAdminCommand request, CancellationToken cancellationToken)
  {
    var newAdmin = Admin.CreateDepartmentAdmin(request.AdminName, request.Email, request.DepartmentId);

    await _repository.AddAsync(newAdmin, cancellationToken);
    await _repository.SaveChangesAsync(cancellationToken);

    return Result<Admin>.Success(newAdmin);
  }
}
