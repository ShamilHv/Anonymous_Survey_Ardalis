namespace Anonymous_Survey_Ardalis.Infrastructure.Email;

/// <summary>
///   Configuration for email server settings
/// </summary>
public class MailserverConfiguration
{
  /// <summary>
  ///   SMTP server address
  /// </summary>
  public string SmtpServer { get; set; } = string.Empty;

  /// <summary>
  ///   SMTP server port
  /// </summary>
  public int SmtpPort { get; set; } = 587; // Default port for TLS/STARTTLS

  /// <summary>
  ///   Username for SMTP authentication
  /// </summary>
  public string Username { get; set; } = string.Empty;

  /// <summary>
  ///   Password for SMTP authentication
  /// </summary>
  public string Password { get; set; } = string.Empty;

  /// <summary>
  ///   Whether to use SSL/TLS for the connection
  /// </summary>
  public bool UseSsl { get; set; } = true;

  /// <summary>
  ///   Email address to use as the sender
  /// </summary>
  public string FromEmail { get; set; } = string.Empty;

  /// <summary>
  ///   Display name to use for the sender
  /// </summary>
  public string FromName { get; set; } = "Anonymous Survey System";
}
