using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class EntityCreationFailedException(string message, string code) : BusinessException(message, code, 404)
{
	public static EntityCreationFailedException ForTree(string reason) =>
		new EntityCreationFailedException($"Nie udało się utworzyć drzewa: {reason}", "TREE_CREATION_FAILED");

	public static EntityCreationFailedException ForMunicipality(string reason) =>
		new EntityCreationFailedException($"Nie udało się utworzyć gminy: {reason}", "MUNICIPALITY_CREATION_FAILED");
}
