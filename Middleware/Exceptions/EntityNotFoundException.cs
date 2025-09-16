using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class EntityNotFoundException(string message, string errorCode) : BusinessException(message, errorCode, 404)
{
	public static EntityNotFoundException ForTree(Guid treeId) =>
			new EntityNotFoundException($"Drzewo o ID {treeId} nie zostało znalezione", "TREE_NOT_FOUND");

	public static EntityNotFoundException ForTreeVote(Guid treeId, Guid userId) =>
		new EntityNotFoundException($"Nie znaleziono istniejącego głosu dla użytkownika o ID {userId} na drzewo o ID {treeId}", "VOTE_NOT_FOUND");

	public static EntityNotFoundException ForUser(Guid userId) =>
			new EntityNotFoundException($"Użytkownik o ID {userId} nie został znaleziony", "USER_NOT_FOUND");

	public static EntityNotFoundException ForAccount(string accountEmail) =>
			new EntityNotFoundException($"Konto o podanym mail'u \"{accountEmail}\" nie zostało znalezione", "ACCOUNT_DOES_NOT_EXISTS");

	public static EntityNotFoundException ForSpecies(Guid speciesId) =>
			new EntityNotFoundException($"Gatunek o ID {speciesId} nie został znaleziony", "SPECIES_NOT_FOUND");

	public static EntityNotFoundException ForMunicipality(Guid municipalityId) =>
			new EntityNotFoundException($"Gmina o ID {municipalityId} nie została znaleziona", "MUNICIPALITY_NOT_FOUND");

	public static EntityNotFoundException ForComment(Guid commentId) =>
			new EntityNotFoundException($"Komentarz o ID {commentId} nie został znaleziony", "COMMENT_NOT_FOUND");

	public static EntityNotFoundException ForCommentVote(Guid commentId, Guid userId) =>
		new EntityNotFoundException($"Nie znaleziono istniejącego głosu dla użytkownika o ID {userId} na komentarz o ID {commentId}", "VOTE_NOT_FOUND");
}
