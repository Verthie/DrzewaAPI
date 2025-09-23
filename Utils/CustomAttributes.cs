using DrzewaAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace DrzewaAPI.Utils;

public class IdempotentActionAttribute : ActionFilterAttribute
{
	private readonly int _cacheMinutes;

	public IdempotentActionAttribute(int cacheMinutes = 10)
	{
		_cacheMinutes = cacheMinutes;
	}

	public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		var idempotencyKey = context.HttpContext.Request.Headers["X-Idempotency-Key"].FirstOrDefault();

		if (string.IsNullOrEmpty(idempotencyKey))
		{
			context.Result = new BadRequestObjectResult("X-Idempotency-Key header is required");
			return;
		}

		var cache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
		var userId = context.HttpContext.User.GetCurrentUserId();
		var actionName = context.ActionDescriptor.DisplayName;
		var cacheKey = $"idempotency:{actionName}:{userId}:{idempotencyKey}";

		// Check for cached result
		if (cache.TryGetValue(cacheKey, out var cachedResult))
		{
			context.Result = (IActionResult?)cachedResult;
			return;
		}

		// Execute the action
		var executedContext = await next();

		// Cache successful results
		if (executedContext.Result is CreatedAtActionResult createdResult)
		{
			cache.Set(cacheKey, createdResult, TimeSpan.FromMinutes(_cacheMinutes));
		}
	}
}