using Anonymous_Survey_Ardalis.Core.DepartmentAggregate;
using Anonymous_Survey_Ardalis.Core.DepartmentAggregate.Specifications;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Departments.Commands.Delete;

public class DeleteDepartmentHandler(IRepository<Department> _repository)
  : ICommandHandler<DeleteDepartmentCommand, Result<int>>
{
  public async Task<Result<int>> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
  {
    var spec = new DepartmentByIdSpec(request.departmentId);
    var department = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (department is null)
    {
      return Result.NotFound();
    }

    await _repository.DeleteAsync(department, cancellationToken);
    await _repository.SaveChangesAsync(cancellationToken);
    return Result.Success(department.Id);
  }
}
