using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class EntityAccessDeniedException(string message, string code) : BusinessException(message, code, 403)
{
	public static EntityAccessDeniedException ForTree(Guid treeId, Guid userId) =>
		new EntityAccessDeniedException($"Użytkownik {userId} nie ma uprawnień do drzewa {treeId}", "TREE_ACCESS_DENIED");

	public static EntityAccessDeniedException ForUser(Guid userId) =>
		new EntityAccessDeniedException($"Brak uprawnień dostępu do użytkownika {userId}", "USER_ACCESS_DENIED");
}
