using DrzewaAPI.Models.Enums;

namespace DrzewaAPI.Extensions;

public static class AttachmentExtensions
{
	public static AttachmentType GetAttachmentType(this string fileExtension)
	{
		return fileExtension.ToLower() switch
		{
			".jpg" or ".jpeg" or ".png" or ".webp" or ".gif" => AttachmentType.Image,
			".mp4" or ".avi" or ".mov" => AttachmentType.Video,
			_ => AttachmentType.Unknown
		};
	}
}
