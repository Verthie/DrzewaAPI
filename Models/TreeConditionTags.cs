using System;

namespace DrzewaAPI.Models;

public class TreeConditionTags
{
	public Guid TreeReportId { get; set; }
	public Guid TagId { get; set; }

	// Navigation Properties
	public required ICollection<TreeReport> TreeReports { get; set; }
	public required ICollection<Tag> Tags { get; set; }
}
