using System;
using DrzewaAPI.Models.Enums;

namespace DrzewaAPI.Models;

public class TreeSubmissionAttachment
{
	public Guid Id { get; set; }
	public Guid TreeSubmissionId { get; set; }
	public required string FileName { get; set; }
	public required string FileUrl { get; set; }
	public AttachmentType Type { get; set; } // Image, Video
	public long FileSize { get; set; }
	public DateTime UploadedAt { get; set; }

	// Navigation Properties
	public TreeSubmission? TreeSubmission { get; set; }
}