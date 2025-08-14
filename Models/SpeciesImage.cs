using System;
using DrzewaAPI.Utils;

namespace DrzewaAPI.Models;

public class SpeciesImage
{
	public required Guid Id { get; set; }
	public required string ImageUrl { get; set; }
	public ImageType Type { get; set; } // Tree, Bark, Leaf, Fruit
	public string? Description { get; set; }

	// Navigation Properties
	public ICollection<TreeSpeciesImages>? TreeSpeciesImages { get; set; }
}
