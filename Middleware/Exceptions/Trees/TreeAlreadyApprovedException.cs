using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class TreeAlreadyApprovedException : BusinessException
{
	public TreeAlreadyApprovedException(Guid treeId)
			: base($"Drzewo o ID {treeId} zostało już zatwierdzone", "TREE_ALREADY_APPROVED", 409)
	{
	}
}