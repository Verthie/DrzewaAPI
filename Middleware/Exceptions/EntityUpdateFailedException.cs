using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class EntityUpdateFailedException(string message, string code) : BusinessException(message, code, 403)
{
	public static EntityUpdateFailedException ForUser(Guid userId, string reason) =>
		new EntityUpdateFailedException($"Nie udało się zaktualizować danych użytkownika {userId}: {reason}", "USER_UPDATE_FAILED");

	public static EntityUpdateFailedException ForMunicipality(Guid municipalityId, string reason) =>
		new EntityUpdateFailedException($"Nie udało się zaktualizować danych gminy {municipalityId}: {reason}", "MUNICIPALITY_UPDATE_FAILED");
}
