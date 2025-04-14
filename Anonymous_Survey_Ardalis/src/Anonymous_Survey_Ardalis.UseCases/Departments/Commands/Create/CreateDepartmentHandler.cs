using Anonymous_Survey_Ardalis.Core.DepartmentAggregate;
using Anonymous_Survey_Ardalis.Core.SubjectAggregate;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Departments.Commands.Create;

public class CreateDepartmentHandler(IRepository<Department> _repository)
  : ICommandHandler<CreateDepartmentCommand, Result<int>>
{
  public async Task<Result<int>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
  {
    var newDepartment = new Department(request.DepartmentName);

    await _repository.AddAsync(newDepartment, cancellationToken);
    await _repository.SaveChangesAsync(cancellationToken);

    return await Task.FromResult(new Result<int>(newDepartment.Id));
  }
}
