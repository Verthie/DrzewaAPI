using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class EntityNotFoundException(string message, string code) : BusinessException(message, code, 404)
{
	public static EntityNotFoundException ForTree(Guid treeId) =>
			new EntityNotFoundException($"Drzewo o ID {treeId} nie zostało znalezione", "TREE_NOT_FOUND");

	public static EntityNotFoundException ForUser(Guid userId) =>
			new EntityNotFoundException($"Użytkownik o ID {userId} nie został znaleziony", "USER_NOT_FOUND");

	public static EntityNotFoundException ForSpecies(Guid speciesId) =>
			new EntityNotFoundException($"Gatunek o ID {speciesId} nie został znaleziony", "SPECIES_NOT_FOUND");

	public static EntityNotFoundException ForMunicipality(Guid municipalityId) =>
			new EntityNotFoundException($"Gmina o ID {municipalityId} nie została znaleziona", "MUNICIPALITY_NOT_FOUND");
}
