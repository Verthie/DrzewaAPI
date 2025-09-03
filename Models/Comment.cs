using System.ComponentModel.DataAnnotations;
using DrzewaAPI.Utils;

namespace DrzewaAPI.Models;

public class Comment
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public Guid TreeSubmissionId { get; set; }
	[Required]
	[MaxLength(3000)]
	public required string Content { get; set; }
	public DateTime DatePosted { get; set; } = DateTime.UtcNow;
	public bool IsLegend { get; set; } = false;

	// Navigation Properties
	public User User { get; set; } = default!;
	public TreeSubmission TreeSubmission { get; set; } = default!;
	public ICollection<CommentVote> CommentVotes { get; set; } = new List<CommentVote>();
}
