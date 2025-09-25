namespace DrzewaAPI.Services;

public interface IAuthService
{
	Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
	Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
	Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
	Task<EmailVerificationResultDto> VerifyEmailAsync(Guid userId);
	Task<EmailVerificationResultDto> ResendVerificationEmailAsync(string email);
	Task SendPasswordResetEmailAsync(string email);
}
