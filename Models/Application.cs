using System;

namespace DrzewaAPI.Models;

public class Application
{
	public required Guid Id { get; set; }
	public required Guid UserId { get; set; }
	public required Guid MunicipalityId { get; set; }
	public required Guid TreeReportId { get; set; }
	public required string PersonalData { get; set; }
	public required string ContactPhone { get; set; }
	public required string ContactEmail { get; set; }
	public string? Justification { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? ExpiresAt { get; set; }
	public string? PdfUrl { get; set; }
	public string? AttachmentsZipUrl { get; set; }

	// Navigation Properties
	public User? User { get; set; }
	public Municipality? Municipality { get; set; }
	public TreeReport? TreeReport { get; set; }
}