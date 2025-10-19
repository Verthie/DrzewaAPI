using System;
using DrzewaAPI.Data;
using DrzewaAPI.Models;
using MaIN.Core.Hub;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Services;

public class GeminiService(ApplicationDbContext _context, ILogger<GeminiService> _logger) : IGeminiService
{
	public async Task<string> GetJustificationAsync(Guid applicationId)
	{
		try
		{
			Application application = await _context.Applications
				.Include(a => a.ApplicationTemplate)
				.Include(t => t.TreeSubmission)
				.FirstOrDefaultAsync(a => a.Id == applicationId)
				?? throw EntityNotFoundException.ForApplication(applicationId);

			TreeSubmission submission = application.TreeSubmission;
			ApplicationTemplate template = application.ApplicationTemplate;

			string predeterminedMessage = $"Z przekazanych danych wyciągnij znaczące informacje odnośnie podanego drzewa, a następnie stwórz uzasadnienie do wniosku z prośbą o jego ochronę na minimum {template.GeminiResponseMinLength} liter i maksimum {template.GeminiResponseMaxLength} liter. Nie dodawaj informacji które nie są istotne dla wniosku, pomijaj rzeczy nieistotne, nie używaj wypunktowywania. Dane: [Gatunek: {submission.Species.PolishName}, Opis gatunku: {submission.Species.Description}, Szacowany wiek: {submission.EstimatedAge}, {(submission.Name != null ? "Potencjalna nazwa: " + submission.Name + "," : "")} Adres: {submission.Location.Address}, {(submission.Health != null ? "Stan zdrowia drzewa:" + "'" + string.Join(", ", submission.Health) + "'," : "")} {(submission.Soil != null ? "Stan gleby:" + "'" + string.Join(", ", submission.Soil) + "'," : "")} {(submission.Environment != null ? "Stan środowiska:" + "'" + string.Join(", ", submission.Environment) + "'," : "")} {(submission.Legend != null ? "Kontekst historyczny:" + "'" + submission.Legend + "'," : "")} {(submission.Description != null ? "Opis drzewa:" + "'" + submission.Description + "'" : "")}]";

			var response = await AIHub.Chat()
				.WithModel("gemini-2.0-flash")
				.WithMessage(predeterminedMessage)
				.CompleteAsync(interactive: true);

			return response.Message.Content;
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas generowania uzasadnienia");
			throw new ServiceException($"Nie udało się wygenerować uzasadnienia", "JUSTIFICATION_GENERATION_ERROR");
		}
	}

	public async Task<string> GetTestMessageAsync()
	{
		try
		{
			var response = await AIHub.Chat()
				.WithModel("gemini-2.0-flash")
				.WithMessage("Opowiedz żart ekologiczny")
				.CompleteAsync(interactive: true);

			return response.Message.Content;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas uzyskiwania wiadomości");
			throw new ServiceException($"Nie udało się uzyskać wiadomości", "MESSAGE_RETRIEVE_ERROR");
		}
	}
}
