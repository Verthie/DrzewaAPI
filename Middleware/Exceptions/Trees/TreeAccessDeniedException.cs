using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class TreeAccessDeniedException : BusinessException
{
	public TreeAccessDeniedException(Guid treeId, Guid userId)
			: base($"Użytkownik {userId} nie ma uprawnień do drzewa {treeId}", "TREE_ACCESS_DENIED", 403)
	{
	}
}
