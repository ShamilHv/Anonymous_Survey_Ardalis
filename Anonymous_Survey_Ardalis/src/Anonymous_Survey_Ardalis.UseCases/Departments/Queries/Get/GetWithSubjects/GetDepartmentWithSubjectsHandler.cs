using Anonymous_Survey_Ardalis.Core.DepartmentAggregate;
using Anonymous_Survey_Ardalis.Core.DepartmentAggregate.Specifications;
using Anonymous_Survey_Ardalis.Core.SubjectAggregate;
using Anonymous_Survey_Ardalis.UseCases.Subjects;
using Ardalis.GuardClauses;
using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;

namespace Anonymous_Survey_Ardalis.UseCases.Departments.Queries.GetWithSubjects;

public class GetDepartmentWithSubjectsHandler(IReadRepository<Department> repository)
  : IRequestHandler<GetDepartmentWithSubjectsQuery, Result<DepartmentWithSubjectsDto>>
{
  public int DepartmentId { get; set; }
  public string DepartmentName { get; set; } = Guard.Against.NullOrEmpty(nameof(DepartmentName));
  public DateTime CreatedAt { get; set; }
  public List<SubjectDto> subjects { get; set; } = new();

  public async Task<Result<DepartmentWithSubjectsDto>> Handle(GetDepartmentWithSubjectsQuery request,
    CancellationToken cancellationToken)
  {
    var spec = new DepartmentWithSubjectsSpec(request.Id);
    var department = await repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (department == null)
    {
      return Result.NotFound();
    }

    var subjectDtos = department.Subjects.Select(MapToSubjectDto).ToList();
    return new DepartmentWithSubjectsDto
    {
      DepartmentId = department.Id,
      DepartmentName = department.DepartmentName,
      CreatedAt = department.CreatedAt,
      subjects = subjectDtos
    };
  }

  private SubjectDto MapToSubjectDto(Subject subject)
  {
    return new SubjectDto(subject.Id, subject.SubjectName, subject.DepartmentId, subject.CreatedAt);
  }
}
