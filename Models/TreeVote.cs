using System;
using DrzewaAPI.Utils;

namespace DrzewaAPI.Models;

public class TreeVote : Vote
{
	public Guid TreeSubmissionId { get; set; }

	// Navigation Properties
	public TreeSubmission TreeSubmission { get; set; } = default!;
}
