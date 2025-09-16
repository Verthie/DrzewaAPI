using System;
using DrzewaAPI.Data;
using DrzewaAPI.Dtos.Application;
using DrzewaAPI.Extensions;
using DrzewaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Services;

public class ApplicationTemplateService(ApplicationDbContext _context, ILogger<ApplicationTemplateService> _logger) : IApplicationTemplateService
{
	public async Task<List<ApplicationTemplateDto>> GetAllTemplatesAsync()
	{
		try
		{
			List<ApplicationTemplateDto> templates = await _context.ApplicationTemplates
					.OrderBy(t => t.Municipality)
					.ThenBy(t => t.Name)
					.Select(t => t.MapToDto())
					.ToListAsync();

			return templates;
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania listy szablonów");
			throw new ServiceException($"Nie można pobrać listy szablonów", "TEMPLATE_FETCH_ERROR");
		}
	}

	public async Task<List<ApplicationTemplateDto>> GetActiveTemplatesAsync()
	{
		try
		{
			List<ApplicationTemplateDto> templates = await _context.ApplicationTemplates
					.Where(t => t.IsActive)
					.OrderBy(t => t.Municipality)
					.ThenBy(t => t.Name)
					.Select(t => t.MapToDto())
					.ToListAsync();

			return templates;
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania listy aktywnych szablonów");
			throw new ServiceException($"Nie można pobrać listy aktywnych szablonów", "TEMPLATE_FETCH_ERROR");
		}
	}

	public async Task<ApplicationTemplateDto> GetTemplateByIdAsync(Guid id)
	{
		try
		{
			ApplicationTemplate template = await _context.ApplicationTemplates
					.FirstOrDefaultAsync(t => t.Id == id)
					?? throw EntityNotFoundException.ForTemplate(id);

			return template.MapToDto();
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania szablonu {TemplateId}", id);
			throw new ServiceException($"Nie można pobrać szablonu {id}", "TEMPLATE_FETCH_ERROR");
		}
	}

	public async Task<List<ShortApplicationTemplateDto>> GetTemplatesByMunicipalityIdAsync(Guid municipalityId)
	{
		try
		{
			bool municipalityExists = await _context.Municipalities.AnyAsync(m => m.Id == municipalityId);
			if (!municipalityExists) throw EntityNotFoundException.ForMunicipality(municipalityId);

			List<ShortApplicationTemplateDto> templates = await _context.ApplicationTemplates
					.Include(t => t.Municipality)
					.Where(t => t.MunicipalityId == municipalityId && t.IsActive)
					.OrderBy(t => t.Name)
					.Select(t => t.MapToShortDto())
					.ToListAsync();

			return templates;
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania listy szablonów");
			throw new ServiceException($"Nie można pobrać listy szablonów", "TEMPLATE_FETCH_ERROR");
		}
	}

	public async Task<ApplicationTemplateDto> CreateTemplateAsync(CreateApplicationTemplateDto createDto)
	{
		try
		{
			bool municipalityExists = await _context.Municipalities.AnyAsync(m => m.Id == createDto.MunicipalityId);
			if (!municipalityExists) throw EntityNotFoundException.ForMunicipality(createDto.MunicipalityId);

			ApplicationTemplate template = new ApplicationTemplate
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
		catch (BusinessException)
		{
			throw;
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError(ex, "Błąd bazy danych podczas tworzenia szablonu");
			throw EntityCreationFailedException.ForTree("Błąd podczas zapisu do bazy danych");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas tworzenia szablonu");
			throw EntityCreationFailedException.ForTree("Nieoczekiwany błąd systemu");
		}
	}

	public async Task<ApplicationTemplateDto> UpdateTemplateAsync(Guid id, UpdateApplicationTemplateDto updateDto)
	{
		try
		{
			ApplicationTemplate template = await _context.ApplicationTemplates
					.FirstOrDefaultAsync(t => t.Id == id)
					?? throw EntityNotFoundException.ForTemplate(id);

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
		catch (BusinessException)
		{
			throw;
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError(ex, "Błąd podczas wprowadzania danych do bazy");
			throw EntityUpdateFailedException.ForTemplate(id, "Błąd podczas zapisu do bazy danych");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas aktualizacji danych szablonu");
			throw EntityUpdateFailedException.ForTemplate(id, "Nieoczekiwany błąd systemu");
		}
	}

	public async Task DeleteTemplateAsync(Guid id)
	{
		ApplicationTemplate template = await _context.ApplicationTemplates
				.FirstOrDefaultAsync(t => t.Id == id)
				?? throw EntityNotFoundException.ForTemplate(id);

		// Soft delete - mark as inactive
		template.IsActive = false;
		template.LastModifiedDate = DateTime.UtcNow;

		await _context.SaveChangesAsync();
	}
}