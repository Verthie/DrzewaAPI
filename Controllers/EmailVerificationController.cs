using DrzewaAPI.Models;
using DrzewaAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrzewaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmailVerificationController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    private readonly ITokenService _tokenService;
    // private readonly IUserService _userService;
    private readonly ILogger<EmailVerificationController> _logger;

    public EmailVerificationController(
        IAuthService authService,
        IEmailService emailService,
        ITokenService tokenService,
        // IUserService userService,
        ILogger<EmailVerificationController> logger)
    {
        _authService = authService;
        _emailService = emailService;
        _tokenService = tokenService;
        // _userService = userService;
        _logger = logger;
    }

    [HttpGet("verify")]
    public async Task<IActionResult> VerifyEmailFromLink([FromQuery] string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            _logger.LogWarning("Email verification attempted with empty token");
            return BadRequest(new EmailVerificationResultDto(false, "Brak tokenu weryfikacyjnego"));
        }

        _logger.LogInformation($"Attempting to verify email with token: {token[..10]}...");

        var verificationToken = await _tokenService.ValidateTokenAsync(token, EmailTokenType.EmailVerification);

        await _authService.VerifyEmailAsync(verificationToken.UserId);

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

        await _tokenService.MarkTokenAsUsedAsync(token, ipAddress, userAgent);

        // Sending welcome email
        try
        {
            await _emailService.SendWelcomeEmailAsync(
                verificationToken.User.Email,
                verificationToken.User.FullName
            );
            _logger.LogInformation($"Welcome email sent to {verificationToken.User.Email}");
        }
        catch (Exception emailEx)
        {
            _logger.LogError(emailEx, $"Failed to send welcome email to {verificationToken.User.Email}");
        }

        _logger.LogInformation($"Email verified successfully for user {verificationToken.UserId}");

        // Forwarding to frontend with success monit
        var frontendUrl = GetFrontendUrl();
        return Redirect($"{frontendUrl}?emailVerified=true");
    }

    // [HttpPost("verify")]
    // public async Task<ActionResult<EmailVerificationResultDto>> VerifyEmail([FromBody] VerifyEmailDto req)
    // {
    //     EmailVerificationToken token = await _tokenService.ValidateTokenAsync(req.Token, EmailTokenType.EmailVerification);

    //     await _authService.VerifyEmailAsync(token.UserId);

    //     var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
    //     var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

    //     await _tokenService.MarkTokenAsUsedAsync(req.Token, ipAddress, userAgent);
    //     await _emailService.SendWelcomeEmailAsync(token.User.Email, token.User.FullName);

    //     return Ok(new EmailVerificationResultDto(true, "Email został pomyślnie zweryfikowany"));
    // }

    [HttpPost("resend")]
    public async Task<ActionResult<EmailVerificationResultDto>> ResendVerificationEmail([FromBody] SendVerificationEmailDto req)
    {
        var result = await _authService.ResendVerificationEmailAsync(req.Email);

        return Ok(result);
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult<EmailVerificationResultDto>> ForgotPassword([FromBody] SendVerificationEmailDto req)
    {
        await _authService.SendPasswordResetEmailAsync(req.Email);

        return Ok(new EmailVerificationResultDto(true, "Jeśli konto z tym adresem email istnieje, wysłano instrukcje resetowania hasła"));
    }

    [HttpGet("reset-password")]
    public async Task<ActionResult<EmailVerificationResultDto>> ResetPassword([FromQuery] string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            var errorUrl = GetFrontendUrl();
            return Redirect($"{errorUrl}/reset-password?error=Brak tokenu resetowania hasła");
        }

        var resetToken = await _tokenService.ValidateTokenAsync(token, EmailTokenType.PasswordReset);

        var frontendUrl = GetFrontendUrl();
        return Redirect($"{frontendUrl}/reset-password?token={token}&email={resetToken.User.Email}");
    }

    // [HttpPost("reset-password")]
    // public async Task<ActionResult<EmailVerificationResultDto>> ResetPassword([FromBody] UpdatePasswordDto req)
    // {
    //     EmailVerificationToken token = await _tokenService.ValidateTokenAsync(req.Token, EmailTokenType.PasswordReset);

    //     await _userService.UpdatePasswordAsync(token.UserId, req.NewPassword);

    //     var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
    //     var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

    //     await _tokenService.MarkTokenAsUsedAsync(req.Token, ipAddress, userAgent);
    //     await _emailService.SendPasswordChangedNotificationAsync(token.User.Email, token.User.FullName);

    //     return Ok(new EmailVerificationResultDto(true, "Hasło zostało pomyślnie zmienione"));
    // }

    private string GetFrontendUrl()
    {
        // Pobierz URL frontendu z konfiguracji
        var frontendUrl = HttpContext.RequestServices
            .GetRequiredService<IConfiguration>()["App:FrontendUrl"];

        // Fallback na baseURL jeśli frontendUrl nie jest skonfigurowany
        return frontendUrl ?? HttpContext.RequestServices
            .GetRequiredService<IConfiguration>()["App:BaseUrl"] ?? "https://localhost:3000";
    }
}

