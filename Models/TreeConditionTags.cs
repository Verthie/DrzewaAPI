using System;

namespace DrzewaAPI.Models;

public class TreeConditionTags
{
	public required Guid TreeReportId { get; set; }
	public required Guid TagId { get; set; }

	// Navigation Properties
	public TreeReport? TreeReports { get; set; }
	public Tag? Tags { get; set; }
}
