using System;
using DrzewaAPI.Models;
using DrzewaAPI.Models.Enums;

namespace DrzewaAPI.Extensions;

public static class TreeSpeciesImageExtensions
{
	public static string GenerateAltText(this ImageType type, string? speciesName = null)
	{
		return type switch
		{
			ImageType.Tree => $"{speciesName ?? "Gatunek"} - drzewo",
			ImageType.Bark => $"{speciesName ?? "Gatunek"} - kora",
			ImageType.Leaf => $"{speciesName ?? "Gatunek"} - liście",
			ImageType.Fruit => $"{speciesName ?? "Gatunek"} - owoce",
			_ => $"{speciesName ?? "Gatunek"} - obrazek"
		};
	}
}
