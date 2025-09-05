using System;
using DrzewaAPI.Data;
using DrzewaAPI.Dtos.Application;
using DrzewaAPI.Dtos.Municipality;
using DrzewaAPI.Extensions;
using DrzewaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Services;

public class MunicipalityService(ApplicationDbContext _context) : IMunicipalityService
{
	public async Task<List<MunicipalityDto>> GetAllMunicipalitiesAsync()
	{
		var municipalities = await _context.Municipalities
				.Include(m => m.ApplicationTemplates.Where(t => t.IsActive))
				.Include(m => m.Applications)
				.OrderBy(m => m.Province)
				.ThenBy(m => m.Name)
				.Select(m => MapMunicipalityToDto(m))
				.ToListAsync();

		return municipalities;
	}

	public async Task<MunicipalityDto?> GetMunicipalityByIdAsync(Guid id)
	{
		var municipality = await _context.Municipalities
				.Include(m => m.ApplicationTemplates.Where(t => t.IsActive))
				.Include(m => m.Applications)
				.FirstOrDefaultAsync(m => m.Id == id);

		return municipality != null ? MapMunicipalityToDto(municipality) : null;
	}

	public async Task<MunicipalityDto> CreateMunicipalityAsync(CreateMunicipalityDto createDto)
	{
		// Check if municipality with same name already exists
		var existingMunicipality = await _context.Municipalities
				.FirstOrDefaultAsync(m => m.Name.ToLower() == createDto.Name.ToLower());

		if (existingMunicipality != null)
			throw new ArgumentException("Gmina o podanej nazwie już istnieje");

		var municipality = new Municipality
		{
			Id = Guid.NewGuid(),
			Name = createDto.Name,
			Address = createDto.Address,
			City = createDto.City,
			Province = createDto.Province,
			PostalCode = createDto.PostalCode,
			Phone = createDto.Phone,
			Email = createDto.Email,
			Website = createDto.Website,
			CreatedDate = DateTime.UtcNow
		};

		_context.Municipalities.Add(municipality);
		await _context.SaveChangesAsync();

		return MapMunicipalityToDto(municipality);
	}

	public async Task<MunicipalityDto?> UpdateMunicipalityAsync(Guid id, UpdateMunicipalityDto updateDto)
	{
		var municipality = await _context.Municipalities
				.Include(m => m.ApplicationTemplates)
				.Include(m => m.Applications)
				.FirstOrDefaultAsync(m => m.Id == id);

		if (municipality == null)
			return null;

		// Check for name conflicts if name is being changed
		if (updateDto.Name != null && updateDto.Name.ToLower() != municipality.Name.ToLower())
		{
			var existingMunicipality = await _context.Municipalities
					.FirstOrDefaultAsync(m => m.Name.ToLower() == updateDto.Name.ToLower() && m.Id != id);

			if (existingMunicipality != null)
				throw new ArgumentException("Gmina o podanej nazwie już istnieje");

			municipality.Name = updateDto.Name;
		}

		if (updateDto.Address != null)
			municipality.Address = updateDto.Address;

		if (updateDto.City != null)
			municipality.City = updateDto.City;

		if (updateDto.Province != null)
			municipality.Province = updateDto.Province;

		if (updateDto.PostalCode != null)
			municipality.PostalCode = updateDto.PostalCode;

		if (updateDto.Phone != null)
			municipality.Phone = updateDto.Phone;

		if (updateDto.Email != null)
			municipality.Email = updateDto.Email;

		if (updateDto.Website != null)
			municipality.Website = updateDto.Website;

		municipality.LastModifiedDate = DateTime.UtcNow;

		await _context.SaveChangesAsync();

		return MapMunicipalityToDto(municipality);
	}

	public async Task<bool> DeleteMunicipalityAsync(Guid id)
	{
		var municipality = await _context.Municipalities
				.Include(m => m.Applications)
				.Include(m => m.ApplicationTemplates)
				.FirstOrDefaultAsync(m => m.Id == id);

		if (municipality == null)
			return false;

		_context.Municipalities.Remove(municipality);

		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<List<ApplicationTemplateDto>> GetMunicipalityTemplatesAsync(Guid municipalityId)
	{
		var templates = await _context.ApplicationTemplates
				.Include(t => t.Municipality)
				.Where(t => t.MunicipalityId == municipalityId && t.IsActive)
				.OrderBy(t => t.Name)
				.Select(t => t.MapToDto())
				.ToListAsync();

		return templates;
	}

	public async Task<List<ApplicationDto>> GetMunicipalityApplicationsAsync(Guid municipalityId)
	{
		var applications = await _context.Applications
				.Include(a => a.User)
				.Include(a => a.TreeSubmission)
						.ThenInclude(ts => ts.Species)
				.Include(a => a.ApplicationTemplate)
				.Include(a => a.Municipality)
				.Where(a => a.MunicipalityId == municipalityId)
				.OrderByDescending(a => a.CreatedDate)
				.Select(a => a.MapToDto())
				.ToListAsync();

		return applications;
	}

	private static MunicipalityDto MapMunicipalityToDto(Municipality municipality)
	{
		return new MunicipalityDto
		{
			Id = municipality.Id,
			Name = municipality.Name,
			Address = municipality.Address,
			City = municipality.City,
			Province = municipality.Province,
			PostalCode = municipality.PostalCode,
			Phone = municipality.Phone,
			Email = municipality.Email,
			Website = municipality.Website,
			CreatedDate = municipality.CreatedDate,
			LastModifiedDate = municipality.LastModifiedDate,
		};
	}
}
