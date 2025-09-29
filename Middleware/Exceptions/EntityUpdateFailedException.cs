using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class EntityUpdateFailedException(string message, string errorCode) : BusinessException(message, errorCode, 403)
{
	public static EntityUpdateFailedException ForUser(Guid userId, string reason) =>
		new EntityUpdateFailedException($"Nie udało się zaktualizować danych użytkownika {userId}: {reason}", "USER_UPDATE_FAILED");

	public static EntityUpdateFailedException ForCommune(Guid communeId, string reason) =>
		new EntityUpdateFailedException($"Nie udało się zaktualizować danych gminy {communeId}: {reason}", "COMMUNE_UPDATE_FAILED");

	public static EntityUpdateFailedException ForTemplate(Guid templateId, string reason) =>
		new EntityUpdateFailedException($"Nie udało się zaktualizować danych szablonu {templateId}: {reason}", "TEMPLATE_UPDATE_FAILED");

	public static EntityUpdateFailedException ForApplication(Guid applicationId, string reason) =>
		new EntityUpdateFailedException($"Nie udało się zaktualizować danych wniosku {applicationId}: {reason}", "APPLICATION_UPDATE_FAILED");

	public static EntityUpdateFailedException ForTree(Guid treeId, string reason) =>
		new EntityUpdateFailedException($"Nie udało się zaktualizować danych drzewa {treeId}: {reason}", "TREE_UPDATE_FAILED");

	public static EntityUpdateFailedException ForSpecies(Guid speciesId, string reason) =>
		new EntityUpdateFailedException($"Nie udało się zaktualizować danych gatunku {speciesId}: {reason}", "SPECIES_UPDATE_FAILED");
}
