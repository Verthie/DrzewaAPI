using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class UserUpdateFailedException : BusinessException
{
	public UserUpdateFailedException(Guid userId, string reason)
			: base($"Nie udało się zauktualizować danych użytkownika {userId}: {reason}", "USER_UPDATE_FAILED", 403)
	{
	}
}
