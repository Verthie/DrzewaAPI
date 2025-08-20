using System;

namespace DrzewaAPI.Models;

public class TreeConditionTags
{
	public Guid TreeSubmissionId { get; set; }
	public Guid TagId { get; set; }

	// Navigation Properties
	public TreeSubmission? TreeSubmissions { get; set; }
	public Tag? Tags { get; set; }
}
