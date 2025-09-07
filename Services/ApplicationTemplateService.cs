using System;
using DrzewaAPI.Data;
using DrzewaAPI.Dtos.Application;
using DrzewaAPI.Extensions;
using DrzewaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Services;

public class ApplicationTemplateService(ApplicationDbContext _context) : IApplicationTemplateService
{
	public async Task<List<ApplicationTemplateDto>> GetAllTemplatesAsync()
	{
		var templates = await _context.ApplicationTemplates
				.OrderBy(t => t.Municipality)
				.ThenBy(t => t.Name)
				.Select(t => t.MapToDto())
				.ToListAsync();

		return templates;
	}

	public async Task<List<ApplicationTemplateDto>> GetActiveTemplatesAsync()
	{
		var templates = await _context.ApplicationTemplates
				.Where(t => t.IsActive)
				.OrderBy(t => t.Municipality)
				.ThenBy(t => t.Name)
				.Select(t => t.MapToDto())
				.ToListAsync();

		return templates;
	}

	public async Task<List<ApplicationTemplateDto>> GetTemplatesByMunicipalityAsync(Guid municipalityId)
	{
		var templates = await _context.ApplicationTemplates
				.Where(t => t.MunicipalityId == municipalityId)
				.Select(t => t.MapToDto())
				.ToListAsync();

		return templates;
	}

	public async Task<ApplicationTemplateDto?> GetTemplateByIdAsync(Guid id)
	{
		var template = await _context.ApplicationTemplates
				.FirstOrDefaultAsync(t => t.Id == id);

		return template != null ? template.MapToDto() : null;
	}

	public async Task<List<ShortApplicationTemplateDto>> GetTemplatesByMunicipalityIdAsync(Guid municipalityId)
	{
		var templates = await _context.ApplicationTemplates
				.Include(t => t.Municipality)
				.Where(t => t.MunicipalityId == municipalityId && t.IsActive)
				.OrderBy(t => t.Name)
				.Select(t => t.MapToShortDto())
				.ToListAsync();

		return templates;
	}

	public async Task<ApplicationTemplateDto> CreateTemplateAsync(CreateApplicationTemplateDto createDto)
	{
		var template = new ApplicationTemplate
		{
			Id = Guid.NewGuid(),
			Name = createDto.Name,
			MunicipalityId = createDto.MunicipalityId,
			Description = createDto.Description,
			HtmlTemplate = createDto.HtmlTemplate,
			Fields = createDto.Fields.ToList(),
			IsActive = true,
			CreatedDate = DateTime.UtcNow
		};

		_context.ApplicationTemplates.Add(template);
		await _context.SaveChangesAsync();

		return template.MapToDto();
	}

	public async Task<ApplicationTemplateDto?> UpdateTemplateAsync(Guid id, UpdateApplicationTemplateDto updateDto)
	{
		var template = await _context.ApplicationTemplates
				.FirstOrDefaultAsync(t => t.Id == id);

		if (template == null)
			return null;

		template.Name = updateDto.Name;
		template.MunicipalityId = updateDto.MunicipalityId;
		template.Description = updateDto.Description;
		template.HtmlTemplate = updateDto.HtmlTemplate;
		template.Fields = updateDto.Fields.ToList();
		template.IsActive = updateDto.IsActive;

		template.LastModifiedDate = DateTime.UtcNow;

		await _context.SaveChangesAsync();

		return template.MapToDto();
	}

	public async Task<bool> DeleteTemplateAsync(Guid id)
	{
		var template = await _context.ApplicationTemplates
				.FirstOrDefaultAsync(t => t.Id == id);

		if (template == null)
			return false;

		// Soft delete - mark as inactive
		template.IsActive = false;
		template.LastModifiedDate = DateTime.UtcNow;

		await _context.SaveChangesAsync();
		return true;
	}
}