using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class EntityNotFoundException(string message, string errorCode) : BusinessException(message, errorCode, 404)
{
	public static EntityNotFoundException ForTree(Guid treeId) =>
			new EntityNotFoundException($"Drzewo o ID {treeId} nie zostało znalezione", "TREE_NOT_FOUND");

	public static EntityNotFoundException ForUser(Guid userId) =>
			new EntityNotFoundException($"Użytkownik o ID {userId} nie został znaleziony", "USER_NOT_FOUND");

	public static EntityNotFoundException ForAccount(string accountEmail) =>
			new EntityNotFoundException($"Konto o podanym mail'u \"{accountEmail}\" nie zostało znalezione", "ACCOUNT_DOES_NOT_EXISTS");

	public static EntityNotFoundException ForSpecies(Guid speciesId) =>
			new EntityNotFoundException($"Gatunek o ID {speciesId} nie został znaleziony", "SPECIES_NOT_FOUND");

	public static EntityNotFoundException ForCommune(Guid communeId) =>
			new EntityNotFoundException($"Gmina o ID {communeId} nie została znaleziona", "COMMUNE_NOT_FOUND");

	public static EntityNotFoundException ForTemplate(Guid templateId) =>
			new EntityNotFoundException($"Szablon o ID {templateId} nie został znaleziony", "TEMPLATE_NOT_FOUND");

	public static EntityNotFoundException ForApplication(Guid applicationId) =>
			new EntityNotFoundException($"Aplikacja o ID {applicationId} nie została znaleziona", "APPLICATION_NOT_FOUND");

	public static EntityNotFoundException ForUserApplication(Guid applicationId, Guid userId) =>
		new EntityNotFoundException($"Nie znaleziono wniosku o ID {applicationId} przypisanego do użytkownika o ID {userId}", "APPLICATION_NOT_FOUND");
}
