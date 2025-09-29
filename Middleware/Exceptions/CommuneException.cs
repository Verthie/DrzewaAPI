using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class CommuneException(string message, string errorCode, int statusCode = 400) : BusinessException(message, errorCode, statusCode)
{
	public static CommuneException ForExistingCreation(string communeName) =>
		new CommuneException($"Gmina o nazwie \"{communeName}\" już istnieje", "COMMUNE_ALREADY_EXISTS", 409);
}
