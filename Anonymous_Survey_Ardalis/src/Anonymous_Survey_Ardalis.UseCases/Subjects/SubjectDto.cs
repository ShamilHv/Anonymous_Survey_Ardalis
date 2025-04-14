namespace Anonymous_Survey_Ardalis.UseCases.Subjects;

public record SubjectDto(
  int SubjectId,
  string SubjectName,
  int DepartmentId,
  DateTime CreatedAt
);
