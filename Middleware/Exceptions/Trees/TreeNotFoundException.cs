using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class TreeNotFoundException : BusinessException
{
	public TreeNotFoundException(Guid treeId)
			: base($"Drzewo o ID {treeId} nie zostało znalezione", "TREE_NOT_FOUND", 404)
	{
	}
}
