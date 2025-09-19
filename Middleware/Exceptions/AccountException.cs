using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class AccountException(string message, string errorCode, int statusCode = 400) : BusinessException(message, errorCode, statusCode)
{
	public static AccountException ForIncorrectPassword() =>
	new AccountException("Podane hasło jest nieprawidłowe", "INCORRECT_PASSWORD");
}
