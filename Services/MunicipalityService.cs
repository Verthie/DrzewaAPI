using System;
using DrzewaAPI.Data;
using DrzewaAPI.Dtos.Application;
using DrzewaAPI.Dtos.Municipality;
using DrzewaAPI.Extensions;
using DrzewaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Services;

public class MunicipalityService(ApplicationDbContext _context, ILogger<Municipality> _logger) : IMunicipalityService
{
	public async Task<List<MunicipalityDto>> GetAllMunicipalitiesAsync()
	{
		try
		{
			var municipalities = await _context.Municipalities
					.OrderBy(m => m.Province)
					.ThenBy(m => m.Name)
					.Select(m => MapMunicipalityToDto(m))
					.ToListAsync();

			return municipalities;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania listy gmin");
			throw new ServiceException($"Nie można pobrać listy gmin", "MUNICIPALITY_FETCH_ERROR");
		}
	}

	public async Task<MunicipalityDto> GetMunicipalityByIdAsync(Guid id)
	{
		try
		{
			Municipality municipality = await _context.Municipalities
					.FirstOrDefaultAsync(m => m.Id == id)
					?? throw EntityNotFoundException.ForMunicipality(id);

			return MapMunicipalityToDto(municipality);
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania gminy {MunicipalityId}", id);
			throw new ServiceException($"Nie można pobrać gminy {id}", "MUNICIPALITY_FETCH_ERROR");
		}
	}

	public async Task<MunicipalityDto> CreateMunicipalityAsync(CreateMunicipalityDto createDto)
	{
		try
		{
			// Check if municipality with same name already exists
			Municipality? existingMunicipality = await _context.Municipalities
					.FirstOrDefaultAsync(m => m.Name.ToLower() == createDto.Name.ToLower());

			if (existingMunicipality != null) throw MunicipalityException.ForExistingCreation(existingMunicipality.Name);

			Municipality municipality = new Municipality
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
		catch (BusinessException)
		{
			throw;
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError(ex, "Błąd bazy danych podczas tworzenia gminy");
			throw EntityCreationFailedException.ForMunicipality("Błąd podczas zapisu do bazy danych");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas tworzenia gminy");
			throw EntityCreationFailedException.ForMunicipality("Nieoczekiwany błąd systemu");
		}
	}

	public async Task<MunicipalityDto> UpdateMunicipalityAsync(Guid id, UpdateMunicipalityDto updateDto)
	{
		try
		{
			Municipality municipality = await _context.Municipalities
					.FirstOrDefaultAsync(m => m.Id == id)
					?? throw EntityNotFoundException.ForMunicipality(id);

			// Check for name conflicts if name is being changed
			if (updateDto.Name != null && updateDto.Name.ToLower() != municipality.Name.ToLower())
			{
				var existingMunicipality = await _context.Municipalities
						.FirstOrDefaultAsync(m => m.Name.ToLower() == updateDto.Name.ToLower() && m.Id != id);

				if (existingMunicipality != null) throw MunicipalityException.ForExistingCreation(existingMunicipality.Name);

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
		catch (BusinessException)
		{
			throw;
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError(ex, "Błąd podczas wprowadzania danych do bazy");
			throw EntityUpdateFailedException.ForMunicipality(id, "Błąd podczas zapisu do bazy danych");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas aktualizacji danych gminy: {MunicipalityId}", id);
			throw new ServiceException($"Nie udało się zaktualizować danych użytkownika {id}", "MUNICIPALITY_UPDATE_ERROR");
		}
	}

	public async Task DeleteMunicipalityAsync(Guid id)
	{
		try
		{
			var municipality = await _context.Municipalities
					.FirstOrDefaultAsync(m => m.Id == id) ?? throw EntityNotFoundException.ForMunicipality(id);

			_context.Municipalities.Remove(municipality);

			await _context.SaveChangesAsync();
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas usuwania gminy {MunicipalityId}", id);
			throw new ServiceException($"Nie można usunąć drzewa {id}", "MUNICIPALITY_DELETE_ERROR");
		}
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
