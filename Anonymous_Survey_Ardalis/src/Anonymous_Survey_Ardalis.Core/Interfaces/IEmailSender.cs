using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.Core.CommentAggregate;

namespace Anonymous_Survey_Ardalis.Core.Interfaces;

public interface IEmailSender
{
  Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);

  Task SendWelcomeEmailAsync(Admin admin, string password);

  Task SendSubjectChangeRequestEmailAsync(Admin requestingAdmin, Comment comment, int? suggestedSubjectId,
    string? message);

  Task SendInappropriateCommentReportAsync(Admin reportingAdmin, Comment comment, string? message);
}
