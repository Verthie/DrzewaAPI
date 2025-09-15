using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class UserAccessDeniedException : BusinessException
{
	public UserAccessDeniedException(Guid userId)
			: base($"Brak uprawnień dostępu do użytkownika {userId}", "USER_ACCESS_DENIED", 403)
	{
	}
}
