using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class TreeException(string message, string errorCode, int statusCode = 400) : BusinessException(message, errorCode, statusCode)
{
	public static TreeException ForApprovalFailure(Guid treeId, string reason) =>
		new TreeException($"Nie udało się zatwierdzić drzewa {treeId}: {reason}", "TREE_APPROVAL_FAILED");

	public static TreeException ForExistingApproval(Guid treeId) =>
		new TreeException($"Drzewo o ID {treeId} zostało już zatwierdzone", "TREE_ALREADY_APPROVED", 409);
}
