using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class TreeException(string message, string code, int errorCode = 400) : BusinessException(message, code, errorCode)
{
	public static TreeException ForApprovalFailure(Guid treeId, string reason) =>
		new TreeException($"Nie udało się zatwierdzić drzewa {treeId}: {reason}", "TREE_APPROVAL_FAILED");

	public static TreeException ForExistingApproval(Guid treeId) =>
		new TreeException($"Drzewo o ID {treeId} zostało już zatwierdzone", "TREE_ALREADY_APPROVED", 409);

	public static TreeException ForVoteFailure(Guid treeId, string reason) =>
		new TreeException($"Nie udało się zagłosować na drzewo {treeId}: {reason}", "TREE_VOTE_FAILED", 400);
}
