using System;
using DrzewaAPI.Utils;

namespace DrzewaAPI.Models;

public class TreeReportAttachment
{
	public Guid Id { get; set; }
	public Guid TreeReportId { get; set; }
	public required string FileName { get; set; }
	public required string FileUrl { get; set; }
	public AttachmentType Type { get; set; } // Image, Video
	public long FileSize { get; set; }
	public DateTime UploadedAt { get; set; }

	// Navigation Properties
	public required TreeReport TreeReport { get; set; }
}