using System;
using System.Net;
using System.Net.Mail;

namespace DrzewaAPI.Services;

public class EmailService : IEmailService
{
	private readonly IConfiguration _configuration;
	private readonly ILogger<EmailService> _logger;
	private readonly SmtpClient _smtpClient;

	public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
	{
		_configuration = configuration;
		_logger = logger;

		var host = _configuration["Email:Host"]
							 ?? throw new InvalidOperationException("Email:Host is not configured");

		if (!int.TryParse(_configuration["Email:Port"], out var port))
			throw new InvalidOperationException("Email:Port is missing or invalid");

		if (!bool.TryParse(_configuration["Email:EnableSsl"], out var enableSsl))
			throw new InvalidOperationException("Email:EnableSsl is missing or invalid");

		_smtpClient = new SmtpClient(host)
		{
			Port = port,
			Credentials = new NetworkCredential(
						_configuration["Email:Username"] ?? throw new InvalidOperationException("Email:Username is not configured"),
						_configuration["Email:Password"] ?? throw new InvalidOperationException("Email:Password is not configured")
				),
			EnableSsl = enableSsl
		};
	}

	public async Task SendVerificationEmailAsync(string toEmail, string userName, string verificationToken)
	{
		var verificationUrl = $"{_configuration["App:BaseUrl"]}/api/emailverification/verify?token={verificationToken}";

		var htmlBody = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <title>Potwierdź swój adres email</title>
            </head>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2 style='color: #2e7d32;'>Witaj {userName}!</h2>
                    <p>Dziękujemy za rejestrację w aplikacji <strong>Zgłoś Pomnik</strong>.</p>
                    <p>Aby aktywować swoje konto, kliknij w poniższy przycisk:</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{verificationUrl}' 
                           style='display: inline-block; background-color: #4CAF50; color: white; padding: 14px 25px; text-decoration: none; border-radius: 4px; font-weight: bold;'>
                            Potwierdź email
                        </a>
                    </div>
                    <p>Lub skopiuj i wklej ten link w przeglądarce:</p>
                    <p style='word-break: break-all; background-color: #f5f5f5; padding: 10px; border-radius: 4px;'>{verificationUrl}</p>
                    <p style='color: #666; font-size: 14px;'>Link jest ważny przez 24 godziny.</p>
                    <p style='color: #666; font-size: 14px;'>Jeśli nie rejestrowałeś się w naszej aplikacji, zignoruj tę wiadomość.</p>
                    <hr style='border: none; border-top: 1px solid #eee; margin: 30px 0;'>
                    <p style='color: #666; font-size: 12px;'>Zespół Zgłoś Pomnik</p>
                </div>
            </body>
            </html>";

		var plainTextBody = $@"Witaj {userName}!

		Dziękujemy za rejestrację w aplikacji Zgłoś Pomnik.
		Aby aktywować swoje konto, przejdź na następujący adres:

		{verificationUrl}

		Link jest ważny przez 24 godziny.
		Jeśli nie rejestrowałeś się w naszej aplikacji, zignoruj tę wiadomość.

		Zespół Zgłoś Pomnik";

		await SendEmailAsync(toEmail, "Potwierdź swój adres email - Zgłoś Pomnik", htmlBody, plainTextBody);
	}

	public async Task SendPasswordResetEmailAsync(string toEmail, string userName, string resetToken)
	{
		var resetUrl = $"{_configuration["App:BaseUrl"]}/api/emailverification/reset-password?token={resetToken}";

		var htmlBody = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <title>Reset hasła</title>
            </head>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2 style='color: #d32f2f;'>Reset hasła - {userName}</h2>
                    <p>Otrzymaliśmy prośbę o reset hasła dla Twojego konta w aplikacji <strong>Zgłoś Pomnik</strong>.</p>
                    <p>Aby ustawić nowe hasło, kliknij w poniższy przycisk:</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{resetUrl}' 
                           style='display: inline-block; background-color: #f44336; color: white; padding: 14px 25px; text-decoration: none; border-radius: 4px; font-weight: bold;'>
                            Resetuj hasło
                        </a>
                    </div>
                    <p>Lub skopiuj i wklej ten link w przeglądarce:</p>
                    <p style='word-break: break-all; background-color: #f5f5f5; padding: 10px; border-radius: 4px;'>{resetUrl}</p>
                    <p style='color: #666; font-size: 14px;'><strong>UWAGA:</strong> Link jest ważny tylko przez 1 godzinę ze względów bezpieczeństwa.</p>
                    <p style='color: #666; font-size: 14px;'>Jeśli nie prosiłeś o reset hasła, zignoruj tę wiadomość lub skontaktuj się z nami.</p>
                    <hr style='border: none; border-top: 1px solid #eee; margin: 30px 0;'>
                    <p style='color: #666; font-size: 12px;'>Zespół Zgłoś Pomnik</p>
                </div>
            </body>
            </html>";

		var plainTextBody = $@"Reset hasła - {userName}

		Otrzymaliśmy prośbę o reset hasła dla Twojego konta w aplikacji Zgłoś Pomnik.
		Aby ustawić nowe hasło, przejdź na następujący adres:

		{resetUrl}

		UWAGA: Link jest ważny tylko przez 1 godzinę ze względów bezpieczeństwa.
		Jeśli nie prosiłeś o reset hasła, zignoruj tę wiadomość.

		Zespół Zgłoś Pomnik";

		await SendEmailAsync(toEmail, "Reset hasła - Zgłoś Pomnik", htmlBody, plainTextBody);
	}


	public async Task SendWelcomeEmailAsync(string toEmail, string userName)
	{
		var htmlBody = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <title>Witaj w Zgłoś Pomnik!</title>
            </head>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2 style='color: #2e7d32;'>Witaj w aplikacji Zgłoś Pomnik, {userName}! 🌳</h2>
                    <p>Twoje konto zostało pomyślnie aktywowane i możesz teraz korzystać ze wszystkich funkcji aplikacji.</p>
                    
                    <h3 style='color: #2e7d32;'>Co możesz teraz robić:</h3>
                    <ul style='padding-left: 20px;'>
                        <li>🌲 Dodawać zgłoszenia pomników przyrody</li>
                        <li>🗺️ Przeglądać mapę z lokalizacjami pomników</li>
                        <li>📋 Generować wnioski do gmin o uznanie drzew za pomniki przyrody</li>
                        <li>🏛️ Poznawać różne gatunki drzew dzięki naszemu przewodnikowi</li>
                        <li>👥 Uczestniczyć w społeczności miłośników przyrody</li>
                        <li>⭐ Oceniać i komentować zgłoszenia innych użytkowników</li>
                    </ul>
                    
                    <p>Dziękujemy za dołączenie do naszej społeczności osób dbających o polskie dziedzictwo przyrodnicze!</p>
                    
                    <div style='background-color: #e8f5e8; padding: 15px; border-radius: 4px; margin: 20px 0;'>
                        <p style='margin: 0; color: #2e7d32;'><strong>Wskazówka:</strong> Zacznij od przeglądania mapy pomników w Twojej okolicy!</p>
                    </div>
                    
                    <hr style='border: none; border-top: 1px solid #eee; margin: 30px 0;'>
                    <p style='color: #666; font-size: 12px;'>Zespół Zgłoś Pomnik</p>
                </div>
            </body>
            </html>";

		await SendEmailAsync(toEmail, "Witaj w Zgłoś Pomnik! 🌳", htmlBody);
	}

	public async Task SendPasswordChangedNotificationAsync(string toEmail, string userName)
	{
		var htmlBody = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <title>Hasło zostało zmienione</title>
            </head>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2 style='color: #2e7d32;'>Hasło zostało zmienione - {userName}</h2>
                    <p>Informujemy, że hasło do Twojego konta w aplikacji <strong>Zgłoś Pomnik</strong> zostało pomyślnie zmienione.</p>
                    
                    <div style='background-color: #fff3cd; border: 1px solid #ffeaa7; padding: 15px; border-radius: 4px; margin: 20px 0;'>
                        <p style='margin: 0; color: #856404;'>
                            <strong>⚠️ Jeśli to nie Ty zmieniłeś hasło:</strong><br>
                            Natychmiast skontaktuj się z nami lub zmień hasło ponownie.
                        </p>
                    </div>
                    
                    <p>Zmiana hasła została wykonana: <strong>{DateTime.Now:yyyy-MM-dd HH:mm}</strong></p>
                    
                    <hr style='border: none; border-top: 1px solid #eee; margin: 30px 0;'>
                    <p style='color: #666; font-size: 12px;'>Zespół Zgłoś Pomnik</p>
                </div>
            </body>
            </html>";

		await SendEmailAsync(toEmail, "Hasło zostało zmienione - Zgłoś Pomnik", htmlBody);
	}

	public async Task<bool> SendEmailAsync(string toEmail, string subject, string htmlBody, string? plainTextBody = null)
	{
		try
		{
			var fromAddress = _configuration["Email:FromAddress"]
					 ?? throw new InvalidOperationException("Email:FromAddress is not configured");

			var fromName = _configuration["Email:FromName"]
					?? throw new InvalidOperationException("Email:FromName is not configured");

			_logger.LogInformation($"Attempting to send email to {toEmail} with subject: {subject}");

			var mailMessage = new MailMessage
			{
				From = new MailAddress(fromAddress, fromName),
				Subject = subject,
				Body = htmlBody,
				IsBodyHtml = true
			};

			mailMessage.To.Add(toEmail);

			if (!string.IsNullOrEmpty(plainTextBody))
			{
				var plainView = AlternateView.CreateAlternateViewFromString(plainTextBody, null, "text/plain");
				var htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");

				mailMessage.AlternateViews.Add(plainView);
				mailMessage.AlternateViews.Add(htmlView);
			}

			await _smtpClient.SendMailAsync(mailMessage);
			_logger.LogInformation($"Email sent successfully to {toEmail}");
			return true;
		}
		catch (SmtpException smtpEx)
		{
			_logger.LogError(smtpEx, $"SMTP Error sending email to {toEmail}. Status: {smtpEx.StatusCode}, Message: {smtpEx.Message}");

			// Log specific SMTP error details
			LogSmtpErrorDetails(smtpEx);
			return false;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Failed to send email to {toEmail}");
			return false;
		}
	}

	private void LogSmtpErrorDetails(SmtpException smtpEx)
	{
		var errorMessage = smtpEx.StatusCode switch
		{
			SmtpStatusCode.MailboxBusy => "Mailbox is busy - try again later",
			SmtpStatusCode.MailboxUnavailable => "Mailbox unavailable - check recipient email",
			SmtpStatusCode.TransactionFailed => "Transaction failed - check SMTP configuration",
			SmtpStatusCode.GeneralFailure => "General SMTP failure",
			SmtpStatusCode.CommandUnrecognized => "SMTP command not recognized",
			SmtpStatusCode.SyntaxError => "SMTP syntax error",
			SmtpStatusCode.CommandNotImplemented => "SMTP command not implemented",
			SmtpStatusCode.BadCommandSequence => "Bad SMTP command sequence",
			SmtpStatusCode.UserNotLocalWillForward => "User not local, will forward",
			SmtpStatusCode.UserNotLocalTryAlternatePath => "User not local, try alternate path",
			SmtpStatusCode.InsufficientStorage => "Insufficient storage on server",
			_ => $"Unknown SMTP error: {smtpEx.StatusCode}"
		};

		_logger.LogError($"SMTP Error Details: {errorMessage}");

		// Log configuration (without sensitive data)
		var host = _configuration["Email:Host"];
		var port = _configuration["Email:Port"];
		var enableSsl = _configuration["Email:EnableSsl"];
		var username = _configuration["Email:Username"];

		_logger.LogError($"SMTP Configuration - Host: {host}, Port: {port}, SSL: {enableSsl}, Username: {username}");
	}

	public void Dispose()
	{
		_smtpClient?.Dispose();
	}
}