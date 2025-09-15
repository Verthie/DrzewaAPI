using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class DatabaseConnectionException : BusinessException
{
	public DatabaseConnectionException()
			: base("Błąd połączenia z bazą danych", "DATABASE_CONNECTION_ERROR", 503)
	{
	}
}
