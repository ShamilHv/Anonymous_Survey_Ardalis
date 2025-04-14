using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;

namespace Anonymous_Survey_Ardalis.Web.Subjects;

public class CreateSubjectRequest
{
  public const string Route = "/Subjects";

  [Required] public int DepartmentId { get; set; }

  [Required] public string SubjectName { get; set; } = Guard.Against.NullOrEmpty(nameof(SubjectName));
}
