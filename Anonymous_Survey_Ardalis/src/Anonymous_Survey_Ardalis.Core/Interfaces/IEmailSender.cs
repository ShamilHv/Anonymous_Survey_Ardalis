using Anonymous_Survey_Ardalis.Core.AdminAggregate;

namespace Anonymous_Survey_Ardalis.Core.Interfaces;

public interface IEmailSender
{
  Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
        
  /// <summary>
  /// Sends a welcome email to a newly created admin including their password
  /// </summary>
  Task SendWelcomeEmailAsync(Admin admin, string password);
}
