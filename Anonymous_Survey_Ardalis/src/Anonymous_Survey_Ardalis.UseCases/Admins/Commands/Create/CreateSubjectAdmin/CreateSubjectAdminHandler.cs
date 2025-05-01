using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Admins.Commands.Create;

public class CreateSubjectAdminHandler(IRepository<Admin> _repository)
  : ICommandHandler<CreateSubjectAdminCommand, Result<Admin>>
{
  public async Task<Result<Admin>> Handle(CreateSubjectAdminCommand request, CancellationToken cancellationToken)
  {
    var newAdmin = Admin.CreateSubjectAdmin(request.AdminName, request.Email, request.SubjectId);

    await _repository.AddAsync(newAdmin, cancellationToken);
    await _repository.SaveChangesAsync(cancellationToken);

    return Result<Admin>.Success(newAdmin);
  }
}
