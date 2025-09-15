using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class InvalidIdException : BusinessException
{
	public InvalidIdException(string invalidId)
			: base($"Nieprawidłowy format ID obiektu: {invalidId}", "INVALID_ID", 400)
	{
	}
}
