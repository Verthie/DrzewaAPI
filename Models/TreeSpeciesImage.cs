using System;
using DrzewaAPI.Models.Enums;

namespace DrzewaAPI.Models;

public class TreeSpeciesImage
{
	public Guid Id { get; set; }
	public Guid TreeSpeciesId { get; set; }
	public required string ImageUrl { get; set; }
	public ImageType Type { get; set; } // Tree, Bark, Leaf, Fruit
	public string? AltText { get; set; }

	// Navigation Properties
	public TreeSpecies TreeSpecies { get; set; } = default!;
}
