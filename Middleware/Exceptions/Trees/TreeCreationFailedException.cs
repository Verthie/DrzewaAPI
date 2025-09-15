using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class TreeCreationFailedException : BusinessException
{
	public TreeCreationFailedException(string reason)
					: base($"Nie udało się utworzyć drzewa: {reason}", "TREE_CREATION_FAILED", 400)
	{
	}
}
