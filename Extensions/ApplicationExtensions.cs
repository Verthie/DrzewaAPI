using System;
using DrzewaAPI.Dtos.Application;
using DrzewaAPI.Models;

namespace DrzewaAPI.Extensions;

public static class ApplicationExtensions
{
	public static ApplicationDto MapToDto(this Application application)
	{
		return new ApplicationDto
		{
			Id = application.Id,
			ApplicationTemplateId = application.ApplicationTemplateId,
			FormData = application.FormData,
			Status = application.Status,
			CreatedDate = application.CreatedDate,
			SubmittedDate = application.SubmittedDate,
			ProcessedDate = application.ProcessedDate,
		};
	}

	public static ApplicationTemplateDto MapToDto(this ApplicationTemplate template)
	{
		return new ApplicationTemplateDto
		{
			Id = template.Id,
			MunicipalityId = template.MunicipalityId,
			Name = template.Name,
			Description = template.Description,
			HtmlTemplate = template.HtmlTemplate,
			Fields = template.Fields.ToList(),
			IsActive = template.IsActive,
			CreatedDate = template.CreatedDate,
			LastModifiedDate = template.LastModifiedDate,
		};
	}

	public static ShortApplicationTemplateDto MapToShortDto(this ApplicationTemplate template)
	{
		return new ShortApplicationTemplateDto
		{
			Id = template.Id,
			Name = template.Name,
			Description = template.Description,
			IsActive = template.IsActive,
			CreatedDate = template.CreatedDate,
			LastModifiedDate = template.LastModifiedDate,
		};
	}
}
