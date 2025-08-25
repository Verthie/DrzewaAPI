using System;
using DrzewaAPI.Models.Enums;

namespace DrzewaAPI.Dtos.Species;

public class TreeSpeciesImageDto
{
	public required string ImageUrl { get; set; }
	public ImageType Type { get; set; } // Tree, Bark, Leaf, Fruit
	public string? AltText { get; set; }
}
