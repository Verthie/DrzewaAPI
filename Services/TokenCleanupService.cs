using System;

namespace DrzewaAPI.Services;

public class TokenCleanupService : BackgroundService
{
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger<TokenCleanupService> _logger;

	public TokenCleanupService(IServiceProvider serviceProvider, ILogger<TokenCleanupService> logger)
	{
		_serviceProvider = serviceProvider;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				using var scope = _serviceProvider.CreateScope();
				var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();

				await tokenService.CleanupExpiredTokensAsync();
				_logger.LogInformation("Token cleanup completed successfully");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred during token cleanup");
			}

			// Run cleanup every 6 hours
			await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
		}
	}
}