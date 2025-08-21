using System;

namespace DrzewaAPI.Models.ValueObjects;

public record Traits
{
	public int? MaxHeight { get; set; }
	public string? Lifespan { get; set; }
	public bool NativeToPoland { get; set; } = true;
}
