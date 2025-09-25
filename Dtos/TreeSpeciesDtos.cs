using DrzewaAPI.Models.Enums;

namespace DrzewaAPI.Dtos;

public record TreeSpeciesDto
{
	public Guid Id { get; set; }
	public required string PolishName { get; set; }
	public required string LatinName { get; set; }
	public required string Family { get; set; }
	public string? Description { get; set; }
	public List<string>? IdentificationGuide { get; set; }
	public SeasonalChangesDto? SeasonalChanges { get; set; }
	public List<TreeSpeciesImageDto>? Images { get; set; }
	public TraitsDto? Traits { get; set; }
}

public record CreateTreeSpeciesDto
{
	public required string PolishName { get; set; }
	public required string LatinName { get; set; }
	public required string Family { get; set; }
	public string? Description { get; set; }
	public List<string>? IdentificationGuide { get; set; }
	public SeasonalChangesDto? SeasonalChanges { get; set; }
	public TraitsDto? Traits
	{ get; set; }
}

public record TreeSpeciesImageDto
{
	public required string ImageUrl { get; set; }
	public ImageType Type { get; set; } // Tree, Bark, Leaf, Fruit
	public string? AltText { get; set; }
}

public record SeasonalChangesDto
{
	public required string Spring { get; set; }
	public required string Summer { get; set; }
	public required string Autumn { get; set; }
	public required string Winter { get; set; }
}

public record TraitsDto
{
	public int? MaxHeight { get; set; }
	public string? Lifespan { get; set; }
	public bool NativeToPoland { get; set; } = true;
}
