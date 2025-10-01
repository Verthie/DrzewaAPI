using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class EntityVoteException(string message, string errorCode, int statusCode = 400) : BusinessException(message, errorCode, statusCode)
{
	public static EntityVoteException ForTree(Guid treeId, string reason) =>
		new EntityVoteException($"Nie udało się zagłosować na drzewo {treeId}: {reason}", "TREE_VOTE_FAILED");
}
