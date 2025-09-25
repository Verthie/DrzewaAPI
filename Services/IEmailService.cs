using System;

namespace DrzewaAPI.Services;

public interface IEmailService
{
	Task SendVerificationEmailAsync(string toEmail, string userName, string verificationToken);
	Task SendPasswordResetEmailAsync(string toEmail, string userName, string resetToken);
	Task SendWelcomeEmailAsync(string toEmail, string userName);
	Task SendPasswordChangedNotificationAsync(string toEmail, string userName);
	Task<bool> SendEmailAsync(string toEmail, string subject, string htmlBody, string? plainTextBody = null);
}
