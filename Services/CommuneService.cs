using System;
using DrzewaAPI.Data;
using DrzewaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Services;

public class CommuneService(ApplicationDbContext _context, ILogger<Commune> _logger) : ICommuneService
{
	public async Task<List<CommuneDto>> GetAllCommunesAsync()
	{
		try
		{
			var communes = await _context.Communes
					.OrderBy(m => m.Province)
					.ThenBy(m => m.Name)
					.Select(m => MapCommuneToDto(m))
					.ToListAsync();

			return communes;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania listy gmin");
			throw new ServiceException($"Nie można pobrać listy gmin", "COMMUNE_FETCH_ERROR");
		}
	}

	public async Task<CommuneDto> GetCommuneByIdAsync(Guid id)
	{
		try
		{
			Commune commune = await _context.Communes
					.FirstOrDefaultAsync(m => m.Id == id)
					?? throw EntityNotFoundException.ForCommune(id);

			return MapCommuneToDto(commune);
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania gminy {CommuneId}", id);
			throw new ServiceException($"Nie można pobrać gminy {id}", "COMMUNE_FETCH_ERROR");
		}
	}

	public async Task<CommuneDto> CreateCommuneAsync(CreateCommuneDto createDto)
	{
		try
		{
			// Check if commune with same name already exists
			Commune? existingCommune = await _context.Communes
					.FirstOrDefaultAsync(m => m.Name.ToLower() == createDto.Name.ToLower());

			if (existingCommune != null) throw CommuneException.ForExistingCreation(existingCommune.Name);

			Commune commune = new Commune
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

			_context.Communes.Add(commune);
			await _context.SaveChangesAsync();

			return MapCommuneToDto(commune);
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError(ex, "Błąd bazy danych podczas tworzenia gminy");
			throw EntityCreationFailedException.ForCommune("Błąd podczas zapisu do bazy danych");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas tworzenia gminy");
			throw EntityCreationFailedException.ForCommune("Nieoczekiwany błąd systemu");
		}
	}

	public async Task<CommuneDto> UpdateCommuneAsync(Guid id, UpdateCommuneDto updateDto)
	{
		try
		{
			Commune commune = await _context.Communes
					.FirstOrDefaultAsync(m => m.Id == id)
					?? throw EntityNotFoundException.ForCommune(id);

			// Check for name conflicts if name is being changed
			if (updateDto.Name != null && updateDto.Name.ToLower() != commune.Name.ToLower())
			{
				var existingCommune = await _context.Communes
						.FirstOrDefaultAsync(m => m.Name.ToLower() == updateDto.Name.ToLower() && m.Id != id);

				if (existingCommune != null) throw CommuneException.ForExistingCreation(existingCommune.Name);

				commune.Name = updateDto.Name;
			}

			if (updateDto.Address != null)
				commune.Address = updateDto.Address;

			if (updateDto.City != null)
				commune.City = updateDto.City;

			if (updateDto.Province != null)
				commune.Province = updateDto.Province;

			if (updateDto.PostalCode != null)
				commune.PostalCode = updateDto.PostalCode;

			if (updateDto.Phone != null)
				commune.Phone = updateDto.Phone;

			if (updateDto.Email != null)
				commune.Email = updateDto.Email;

			if (updateDto.Website != null)
				commune.Website = updateDto.Website;

			commune.LastModifiedDate = DateTime.UtcNow;

			await _context.SaveChangesAsync();

			return MapCommuneToDto(commune);
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError(ex, "Błąd podczas wprowadzania danych do bazy");
			throw EntityUpdateFailedException.ForCommune(id, "Błąd podczas zapisu do bazy danych");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas aktualizacji danych gminy: {CommuneId}", id);
			throw new ServiceException($"Nie udało się zaktualizować danych użytkownika {id}", "COMMUNE_UPDATE_ERROR");
		}
	}

	public async Task DeleteCommuneAsync(Guid id)
	{
		try
		{
			var commune = await _context.Communes
					.FirstOrDefaultAsync(m => m.Id == id) ?? throw EntityNotFoundException.ForCommune(id);

			_context.Communes.Remove(commune);

			await _context.SaveChangesAsync();
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas usuwania gminy {CommuneId}", id);
			throw new ServiceException($"Nie można usunąć drzewa {id}", "COMMUNE_DELETE_ERROR");
		}
	}

	private static CommuneDto MapCommuneToDto(Commune commune)
	{
		return new CommuneDto
		{
			Id = commune.Id,
			Name = commune.Name,
			Address = commune.Address,
			City = commune.City,
			Province = commune.Province,
			PostalCode = commune.PostalCode,
			Phone = commune.Phone,
			Email = commune.Email,
			Website = commune.Website,
			CreatedDate = commune.CreatedDate,
			LastModifiedDate = commune.LastModifiedDate,
		};
	}
}
