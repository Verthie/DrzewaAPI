using System;

namespace DrzewaAPI.Models;

public class TreeConditionTags
{
	public Guid TreeReportId { get; set; }
	public Guid TagId { get; set; }

	// Navigation Properties
	public ICollection<TreeReport>? TreeReports { get; set; }
	public ICollection<Tag>? Tags { get; set; }
}
