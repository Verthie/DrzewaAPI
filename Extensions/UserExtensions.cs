using System;
using System.Security.Claims;
using DrzewaAPI.Models;

namespace DrzewaAPI.Extensions;

public static class UserExtensions
{
	public static Guid GetCurrentUserId(this ClaimsPrincipal user)
	{
		ArgumentNullException.ThrowIfNull(user);

		var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user.FindFirst("sub")?.Value;

		if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userGuid))
			throw new InvalidOperationException("User ID claim missing or invalid.");

		return userGuid;
	}
}
