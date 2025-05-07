using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Ardalis.Specification;

namespace Anonymous_Survey_Ardalis.UseCases.Comments.Queries.List;

public class CommentPaginatedSpecification : Specification<Comment>
{
  public CommentPaginatedSpecification(
    int pageNumber,
    int pageSize,
    int? subjectId = null,
    int? departmentId = null,
    IEnumerable<int>? subjectIds = null,
    bool showInappropriate = true)
  {
    // First priority: specific subject ID
    if (subjectId.HasValue)
    {
      Query.Where(c => c.SubjectId == subjectId.Value);
    }
    // Second priority: list of subject IDs (used for department filtering)
    else if (subjectIds != null && subjectIds.Any())
    {
      Query.Where(c => subjectIds.Contains(c.SubjectId));
    }

    // Filter inappropriate comments if needed
    if (!showInappropriate)
    {
      Query.Where(c => c.IsAppropriate);
    }

    // Apply pagination
    Query.Skip((pageNumber - 1) * pageSize)
      .Take(pageSize)
      .OrderByDescending(c => c.CreatedAt);
  }
}
//
// public class CommentPaginatedSpecification : Specification<Comment>
// {
//   public CommentPaginatedSpecification(
//     int pageNumber,
//     int pageSize,
//     int? subjectId = null,
//     int? departmentId = null,
//     IEnumerable<int>? subjectIds = null)
//   {
//     // First priority: specific subject ID
//     if (subjectId.HasValue)
//     {
//       Query.Where(c => c.SubjectId == subjectId.Value);
//     }
//     // Second priority: list of subject IDs (used for department filtering)
//     else if (subjectIds != null && subjectIds.Any())
//     {
//       Query.Where(c => subjectIds.Contains(c.SubjectId));
//     }
//
//     // Apply pagination
//     Query.Skip((pageNumber - 1) * pageSize)
//       .Take(pageSize)
//       .OrderByDescending(c => c.CreatedAt);
//   }
// }
