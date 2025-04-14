namespace Anonymous_Survey_Ardalis.UseCases.Subjects.Queries.List;

public interface IListSubjectQueryService
{
  Task<IEnumerable<SubjectDto>> ListAsync();
}
