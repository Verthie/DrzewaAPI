using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class ServiceException : BusinessException
{
	public ServiceException(string message, string errorCode)
			: base(message, errorCode, 500)
	{
	}
}
