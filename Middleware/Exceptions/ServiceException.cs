using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class ServiceException(string message, string errorCode) : BusinessException(message, errorCode, 500)
{
}
