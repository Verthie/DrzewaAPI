using System;
using DrzewaAPI.Utils;

namespace DrzewaAPI.Models;

public class TreeSpeciesImage
{
	public Guid Id { get; set; }
	public Guid SpeciesId { get; set; }
	public required string ImageUrl { get; set; }
	public ImageType Type { get; set; } // Tree, Bark, Leaf, Fruit
	public string? Description { get; set; }

	// Navigation Properties
	public TreeSpecies? Species { get; set; }
}
