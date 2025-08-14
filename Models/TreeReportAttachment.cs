using System;
using DrzewaAPI.Utils;

namespace DrzewaAPI.Models;

public class TreeReportAttachment
{
	public required Guid Id { get; set; }
	public required Guid TreeReportId { get; set; }
	public required string FileName { get; set; }
	public required string FileUrl { get; set; }
	public AttachmentType Type { get; set; } // Image, Video
	public long FileSize { get; set; }
	public DateTime UploadedAt { get; set; }

	// Navigation Properties
	public TreeReport? TreeReport { get; set; }
}