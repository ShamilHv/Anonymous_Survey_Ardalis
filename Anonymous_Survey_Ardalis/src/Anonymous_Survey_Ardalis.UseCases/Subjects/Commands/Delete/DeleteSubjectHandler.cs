using Anonymous_Survey_Ardalis.Core.SubjectAggregate;
using Anonymous_Survey_Ardalis.Core.SubjectAggregate.Specifications;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Anonymous_Survey_Ardalis.UseCases.Subjects.Commands.Delete;

public class DeleteSubjectHandler(IRepository<Subject> _repository)
  : ICommandHandler<DeleteSubjectCommand, Result<int>>
{
  public async Task<Result<int>> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
  {
    var spec = new SubjectByIdSpec(request.subjectId);
    var subject = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (subject is null)
    {
      return Result.NotFound();
    }

    await _repository.DeleteAsync(subject, cancellationToken);
    await _repository.SaveChangesAsync(cancellationToken);
    return Result.Success(subject.Id);
  }
}
