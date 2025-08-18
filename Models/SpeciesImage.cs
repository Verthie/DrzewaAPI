using System;
using DrzewaAPI.Models.Enums;

namespace DrzewaAPI.Models;

public class SpeciesImage
{
	public Guid Id { get; set; }
	public required string ImageUrl { get; set; }
	public ImageType Type { get; set; } // Tree, Bark, Leaf, Fruit
	public string? Description { get; set; }

	// Navigation Properties
	public ICollection<TreeSpeciesImages>? TreeSpeciesImages { get; set; }
}
