using Anonymous_Survey_Ardalis.UseCases.Subjects;
using Anonymous_Survey_Ardalis.UseCases.Subjects.Queries.List;

namespace Anonymous_Survey_Ardalis.Infrastructure.Data.Queries;

public class ListSubjectsQueryService : IListSubjectQueryService
{
  public Task<IEnumerable<SubjectDto>> ListAsync()
  {
    List<SubjectDto> fakeSubjects =
    [
      new(1, "IT", 1, DateTime.Now),
      new(1, "Finance", 1, DateTime.Now.AddDays(-2).AddHours(-3))
    ];

    return Task.FromResult(fakeSubjects.AsEnumerable());
  }
}
