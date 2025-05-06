using System.Net;
using System.Net.Mail;
using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.Core.AdminAggregate.Specifications;
using Anonymous_Survey_Ardalis.Core.CommentAggregate;
using Anonymous_Survey_Ardalis.Core.Interfaces;
using Anonymous_Survey_Ardalis.Core.SubjectAggregate;
using Ardalis.SharedKernel;
using Microsoft.Extensions.Logging;

namespace Anonymous_Survey_Ardalis.Infrastructure.Email;

public class EmailSender : IEmailSender
{
  private readonly IRepository<Admin> _adminRepository;
  private readonly MailserverConfiguration _config;
  private readonly ILogger<EmailSender> _logger;
  private readonly IRepository<Subject> _subjectRepository;

  public EmailSender(
    ILogger<EmailSender> logger,
    MailserverConfiguration config,
    IRepository<Admin> adminRepository,
    IRepository<Subject> subjectRepository)
  {
    _logger = logger;
    _config = config;
    _adminRepository = adminRepository;
    _subjectRepository = subjectRepository;
  }

  /// <summary>
  ///   Sends a welcome email with password to a newly created admin
  /// </summary>
  public async Task SendWelcomeEmailAsync(Admin admin, string password)
  {
    var subject = "Welcome to Anonymous Survey System - Your Account Details";

    var body = $@"
            <html>
            <body>
                <h2>Welcome to the Anonymous Survey System!</h2>
                <p>Hello {admin.AdminName},</p>
                <p>Your admin account has been created with the following details:</p>
                <ul>
                    <li><strong>Email:</strong> {admin.Email}</li>
                    <li><strong>Role:</strong> {admin.Role}</li>
                    <li><strong>Temporary Password:</strong> {password}</li>
                </ul>
                <p>Please log in using these credentials and change your password as soon as possible.</p>
                <p>Thank you,<br>Anonymous Survey System Team</p>
            </body>
            </html>";

    await SendEmailAsync(admin.Email, subject, body);
  }


  /// <summary>
  ///   Sends an email to the specified recipient
  /// </summary>
  public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
  {
    try
    {
      var message = new MailMessage
      {
        From = new MailAddress(_config.FromEmail, _config.FromName),
        Subject = subject,
        Body = body,
        IsBodyHtml = isHtml
      };

      message.To.Add(to);

      using var client = new SmtpClient(_config.SmtpServer, _config.SmtpPort)
      {
        EnableSsl = _config.UseSsl, Credentials = new NetworkCredential(_config.Username, _config.Password)
      };

      await client.SendMailAsync(message);
      _logger.LogInformation($"Email sent successfully to {to}");
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, $"Failed to send email to {to}");
      throw;
    }
  }


  public async Task SendSubjectChangeRequestEmailAsync(Admin requestingAdmin, Comment comment, int? suggestedSubjectId,
    string? message)
  {
    try
    {
      // Get suggested subject name
      var currentSubject = await _subjectRepository.GetByIdAsync(comment.SubjectId);

      var suggestedSubject = await _subjectRepository.GetByIdAsync(suggestedSubjectId ?? 0);

      if (currentSubject == null || suggestedSubject == null)
      {
        _logger.LogError("Failed to send subject change request email: Subject not found");
        throw new Exception("Subject not found");
      }

      // Get all super admins
      var superAdmins = await _adminRepository.ListAsync(new AdminBySuperAdminRoleSpec());

      if (!superAdmins.Any())
      {
        _logger.LogWarning("No super admins found to send subject change request email");
        throw new Exception("No super admins found");
      }

      var subject = $"Subject Change Request for Comment #{comment.Id}";

      var body = $@"
            <html>
            <body>
                <h2>Subject Change Request</h2>
                <p>A request has been made to change the subject of a comment.</p>
                
                <h3>Request Details:</h3>
                <ul>
                    <li><strong>Requesting Admin:</strong> {requestingAdmin.AdminName} ({requestingAdmin.Email})</li>
                    <li><strong>Admin Role:</strong> {requestingAdmin.Role}</li>
                    <li><strong>Comment ID:</strong> {comment.Id}</li>
                    <li><strong>Comment Text:</strong> {comment.CommentText}</li>
                    <li><strong>Current Subject:</strong> {currentSubject.SubjectName} (ID: {currentSubject.Id})</li>
                    <li><strong>Suggested Subject:</strong> {suggestedSubject.SubjectName} (ID: {suggestedSubject.Id})</li>
                </ul>
                
                {(string.IsNullOrEmpty(message) ? "" : $@"
                <h3>Additional Message:</h3>
                <p>{message}</p>")}
                
                <p>To approve this change, please update the comment's subject in the admin dashboard.</p>
                
                <p>Thank you,<br>Anonymous Survey System</p>
            </body>
            </html>";

      // Send to all super admins
      foreach (var superAdmin in superAdmins)
      {
        await SendEmailAsync(superAdmin.Email, subject, body);
      }

      _logger.LogInformation($"Subject change request email sent to {superAdmins.Count} super admins");
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Failed to send subject change request email");
      throw;
    }
  }

  public async Task SendInappropriateCommentReportAsync(Admin reportingAdmin, Comment comment, string? message)
  {
    try
    {
      // Get the subject information
      var subject = await _subjectRepository.GetByIdAsync(comment.SubjectId);

      if (subject == null)
      {
        _logger.LogError("Failed to send inappropriate comment report email: Subject not found");
        throw new Exception("Subject not found");
      }

      // Get all super admins
      var superAdmins = await _adminRepository.ListAsync(new AdminBySuperAdminRoleSpec());

      if (!superAdmins.Any())
      {
        _logger.LogWarning("No super admins found to send inappropriate comment report email");
        throw new Exception("No super admins found");
      }

      var emailSubject = $"Inappropriate Comment Report - Comment #{comment.Id}";

      var body = $@"
        <html>
        <body>
            <h2>Inappropriate Comment Report</h2>
            <p>A comment has been reported as inappropriate.</p>
            
            <h3>Report Details:</h3>
            <ul>
                <li><strong>Reporting Admin:</strong> {reportingAdmin.AdminName} ({reportingAdmin.Email})</li>
                <li><strong>Admin Role:</strong> {reportingAdmin.Role}</li>
                <li><strong>Comment ID:</strong> {comment.Id}</li>
                <li><strong>Comment Text:</strong> {comment.CommentText}</li>
                <li><strong>Subject:</strong> {subject.SubjectName} (ID: {subject.Id})</li>
                <li><strong>Created At:</strong> {comment.CreatedAt}</li>
            </ul>
            
            {(string.IsNullOrEmpty(message) ? "" : $@"
            <h3>Additional Message:</h3>
            <p>{message}</p>")}
            
            <p>Please review this comment in the admin dashboard and take appropriate action.</p>
            
            <p>Thank you,<br>Anonymous Survey System</p>
        </body>
        </html>";

      // Send to all super admins
      foreach (var superAdmin in superAdmins)
      {
        await SendEmailAsync(superAdmin.Email, emailSubject, body);
      }

      _logger.LogInformation($"Inappropriate comment report email sent to {superAdmins.Count} super admins");
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Failed to send inappropriate comment report email");
      throw;
    }
  }
}
