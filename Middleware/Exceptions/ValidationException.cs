using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class ValidationException : BusinessException
{
	public Dictionary<string, string[]> AdditionalData { get; }

	public ValidationException(Dictionary<string, string[]> validationErrors)
			: base("Dane nie przeszły walidacji", "VALIDATION_ERROR", 400)
	{
		AdditionalData = validationErrors;
	}
}
