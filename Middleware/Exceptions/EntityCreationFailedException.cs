using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class EntityCreationFailedException(string message, string errorCode) : BusinessException(message, errorCode, 404)
{
	public static EntityCreationFailedException ForTree(string reason) =>
		new EntityCreationFailedException($"Nie udało się utworzyć drzewa: {reason}", "TREE_CREATION_FAILED");

	public static EntityCreationFailedException ForMunicipality(string reason) =>
		new EntityCreationFailedException($"Nie udało się utworzyć gminy: {reason}", "MUNICIPALITY_CREATION_FAILED");

	public static EntityCreationFailedException ForComment(string reason) =>
		new EntityCreationFailedException($"Nie udało się utworzyć komentarza: {reason}", "COMMENT_CREATION_FAILED");

	public static EntityCreationFailedException ForUser(string reason) =>
		new EntityCreationFailedException($"Nie udało się zarejestrować użytkownika: {reason}", "USER_REGISTRATION_FAILED");

	public static EntityCreationFailedException ForApplication(string reason) =>
		new EntityCreationFailedException($"Nie udało się utworzyć wniosku: {reason}", "APPLICATION_CREATION_FAILED");
}
