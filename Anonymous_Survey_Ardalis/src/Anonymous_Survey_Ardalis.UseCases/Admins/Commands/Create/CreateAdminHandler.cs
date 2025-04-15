using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.Core.DepartmentAggregate;
using Anonymous_Survey_Ardalis.UseCases.Departments.Commands.Create;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Admins.Commands.Create;

public class CreateAdminHandler(IRepository<Admin> _repository)
  : ICommandHandler<CreateAdminCommand, Result<int>>
{
  public async Task<Result<int>> Handle(CreateAdminCommand request, CancellationToken cancellationToken)
  {
    var newAdmin = new Admin(request.AdminName, request.Email, request.SubjectId);

    await _repository.AddAsync(newAdmin, cancellationToken);
    await _repository.SaveChangesAsync(cancellationToken);

    return await Task.FromResult(new Result<int>(newAdmin.Id));
  }
}
