using System.Net;
using System.Net.Mail;
using Anonymous_Survey_Ardalis.Core.AdminAggregate;
using Anonymous_Survey_Ardalis.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Anonymous_Survey_Ardalis.Infrastructure.Email;

public class EmailSender: IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly MailserverConfiguration _config;

        /// <summary>
        /// Sends a welcome email with password to a newly created admin
        /// </summary>
        public async Task SendWelcomeEmailAsync(Admin admin, string password)
        {
          string subject = "Welcome to Anonymous Survey System - Your Account Details";
            
          string body = $@"
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

        public EmailSender(ILogger<EmailSender> logger, MailserverConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        /// <summary>
        /// Sends an email to the specified recipient
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
                    EnableSsl = _config.UseSsl,
                    Credentials = new NetworkCredential(_config.Username, _config.Password)
                };

                await client.SendMailAsync(message);
                _logger.LogInformation($"Email sent successfully to {to}");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {to}");
                throw;
            }
        }
    }

