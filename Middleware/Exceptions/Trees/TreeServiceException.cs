using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class TreeServiceException : BusinessException
{
	public TreeServiceException(string message, string errorCode)
			: base(message, errorCode, 500)
	{
	}
}
