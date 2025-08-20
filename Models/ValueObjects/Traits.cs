using System;

namespace DrzewaAPI.Models.ValueObjects;

public class Traits
{
	public int? MaxHeight { get; set; }
	public string? Lifespan { get; set; }
	public bool NativeToPoland { get; set; } = true;
}
