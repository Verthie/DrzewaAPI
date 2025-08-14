using System;

namespace DrzewaAPI.Models;

public class Tag
{
	public Guid Id { get; set; }
	public required string Name { get; set; }

	// Navigation Properties
	public required ICollection<TreeConditionTags> ConditionTags { get; set; }
}
