using System;

namespace DrzewaAPI.Models;

public class TreeConditionTags
{
	public Guid TreeReportId { get; set; }
	public Guid TagId { get; set; }

	// Navigation Properties
	public TreeReport? TreeReports { get; set; }
	public Tag? Tags { get; set; }
}
