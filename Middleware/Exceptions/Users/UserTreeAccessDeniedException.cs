using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class UserTreeAccessDeniedException : BusinessException
{
	public UserTreeAccessDeniedException(Guid userId)
			: base($"Użytkownik {userId} nie ma dostępu do tej operacji", "TREE_ACCESS_DENIED", 403)
	{
	}
}
