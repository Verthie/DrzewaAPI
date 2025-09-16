using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class ServiceException(string message, string errorCode, int statusCode = 500) : BusinessException(message, errorCode, statusCode)
{
}
