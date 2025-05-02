using Anonymous_Survey_Ardalis.Core.DepartmentAggregate;
using Anonymous_Survey_Ardalis.Core.DepartmentAggregate.Specifications;
using Anonymous_Survey_Ardalis.Core.SubjectAggregate;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Subjects.Commands.Create;

public class CreateCommentHandler(IRepository<Subject> _repository, IRepository<Department> _departmentRepository)
  : ICommandHandler<CreateSubjectCommand, Result<int>>
{
  public async Task<Result<int>> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
  {
    var spec = new DepartmentByIdSpec(request.departmentId);
    var department = await _departmentRepository.FirstOrDefaultAsync(spec, cancellationToken);
    if (department is null)
    {
      return Result.NotFound();
    }

    var newSubject = new Subject(request.subjectName, request.departmentId);

    await _repository.AddAsync(newSubject, cancellationToken);
    await _repository.SaveChangesAsync(cancellationToken);

    return await Task.FromResult(new Result<int>(newSubject.Id));
  }
}
