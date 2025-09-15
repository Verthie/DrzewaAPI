using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class UserNotFoundException : BusinessException
{
	public UserNotFoundException(Guid userId)
			: base($"Użytkownik o ID {userId} nie został znaleziony", "USER_NOT_FOUND", 404)
	{
	}
}
