using DrzewaAPI.Dtos.Auth;

namespace DrzewaAPI.Services;

public interface IAuthService
{
	Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
	Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
}
