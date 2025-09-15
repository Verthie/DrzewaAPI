using System;

namespace DrzewaAPI.Middleware.Exceptions;

public class SpeciesNotFoundException : BusinessException
{
	public SpeciesNotFoundException(Guid speciesId)
			: base($"Gatunek o ID {speciesId} nie został znaleziony", "SPECIES_NOT_FOUND", 404)
	{
	}
}
