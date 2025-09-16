using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class MunicipalityException(string message, string errorCode, int statusCode = 400) : BusinessException(message, errorCode, statusCode)
{
	public static MunicipalityException ForExistingCreation(string municipalityName) =>
		new MunicipalityException($"Gmina o nazwie \"{municipalityName}\" już istnieje", "MUNICIPALITY_ALREADY_EXISTS", 409);
}
