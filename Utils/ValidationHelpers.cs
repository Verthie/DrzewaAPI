using System;
using DrzewaAPI.Dtos.TreeSubmissions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DrzewaAPI.Utils;

public static class ValidationHelpers
{

	/// <summary>
	/// Validates and parses string to Guid. Throws InvalidIdException if incorrect.
	/// </summary>
	public static Guid ValidateAndParseId(string id)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			throw new InvalidIdException("ID nie może być puste");
		}

		if (!Guid.TryParse(id, out var guid))
		{
			throw new InvalidIdException(id);
		}

		return guid;
	}

	/// <summary>
	/// Checks ModelState and throws ValidationException if incorrect.
	/// </summary>
	public static void ValidateModelState(ModelStateDictionary modelState)
	{
		if (!modelState.IsValid)
		{
			var errors = modelState
					.Where(x => x.Value != null && x.Value.Errors.Count > 0)
					.ToDictionary(
							kvp => kvp.Key,
							kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
					);

			throw new ValidationException(errors);
		}
	}
}
