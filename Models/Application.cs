namespace DrzewaAPI.Models;

public class Application
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public Guid TreeSubmissionId { get; set; }
	public Guid ApplicationTemplateId { get; set; }
	public required Dictionary<string, object> FormData { get; set; } = new();
	public ApplicationStatus Status { get; set; } = ApplicationStatus.Draft;
	public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
	public DateTime? SubmittedDate { get; set; }
	public DateTime? ProcessedDate { get; set; }

	public string? GeneratedHtmlContent { get; set; }
	public string? GeneratedPdfPath { get; set; }

	// Navigation Properties
	public User User { get; set; } = default!;
	public TreeSubmission TreeSubmission { get; set; } = default!;
	public ApplicationTemplate ApplicationTemplate { get; set; } = default!;
}
