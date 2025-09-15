using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class TreeApprovalFailedException : BusinessException
{
	public TreeApprovalFailedException(Guid treeId, string reason)
			: base($"Nie udało się zatwierdzić drzewa {treeId}: {reason}", "TREE_APPROVAL_FAILED", 400)
	{
	}
}
