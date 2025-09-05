using System;
using System.ComponentModel.DataAnnotations;
using DrzewaAPI.Data;
using DrzewaAPI.Dtos.Application;
using DrzewaAPI.Extensions;
using DrzewaAPI.Models;
using DrzewaAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Services;

public class ApplicationService(ApplicationDbContext _context, IFileGenerationService _fileGenerationService) : IApplicationService
{
	public async Task<List<ApplicationDto>> GetUserApplicationsAsync(Guid userId)
	{
		var applications = await _context.Applications
				.Include(a => a.ApplicationTemplate)
				.Include(a => a.TreeSubmission)
						.ThenInclude(ts => ts.Species)
				.Where(a => a.UserId == userId)
				.OrderByDescending(a => a.CreatedDate)
				.Select(a => a.MapToDto())
				.ToListAsync();

		return applications;
	}

	public async Task<ApplicationDto?> GetApplicationByIdAsync(Guid id, Guid userId)
	{
		var application = await _context.Applications
				.Include(a => a.ApplicationTemplate)
				.Include(a => a.TreeSubmission)
						.ThenInclude(ts => ts.Species)
				.Include(a => a.User)
				.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

		return application != null ? application.MapToDto() : null;
	}

	public async Task<ApplicationDto> CreateApplicationAsync(Guid userId, CreateApplicationDto createDto)
	{
		// Verify that tree submission belongs to user
		var treeSubmission = await _context.TreeSubmissions
				.Include(ts => ts.Species)
				.FirstOrDefaultAsync(ts => ts.Id == createDto.TreeSubmissionId && ts.UserId == userId);

		if (treeSubmission == null)
			throw new ArgumentException("Zgłoszenie drzewa nie zostało znalezione lub nie należy do użytkownika");

		// Verify that application template exists
		var template = await _context.ApplicationTemplates
				.FirstOrDefaultAsync(at => at.Id == createDto.ApplicationTemplateId);

		if (template == null)
			throw new ArgumentException("Szablon wniosku nie został znaleziony lub jest nieaktywny");

		var application = new Application
		{
			Id = Guid.NewGuid(),
			UserId = userId,
			TreeSubmissionId = createDto.TreeSubmissionId,
			ApplicationTemplateId = createDto.ApplicationTemplateId,
			FormData = new Dictionary<string, object>(),
			Status = ApplicationStatus.Draft,
			CreatedDate = DateTime.UtcNow
		};

		_context.Applications.Add(application);
		await _context.SaveChangesAsync();

		application.TreeSubmission = treeSubmission;
		application.ApplicationTemplate = template;

		return application.MapToDto();
	}

	public async Task<ApplicationDto?> UpdateApplicationAsync(Guid id, Guid userId, UpdateApplicationDto updateDto)
	{
		var application = await _context.Applications
				.Include(a => a.ApplicationTemplate)
				.Include(a => a.TreeSubmission)
						.ThenInclude(ts => ts.Species)
				.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

		if (application == null)
			return null;

		// Can only update draft applications
		if (application.Status != ApplicationStatus.Draft)
			throw new InvalidOperationException("Można edytować tylko wnioski w stanie roboczym");

		if (updateDto.FormData != null)
			application.FormData = updateDto.FormData;

		await _context.SaveChangesAsync();

		return application.MapToDto();
	}

	public async Task<bool> DeleteApplicationAsync(Guid id, Guid userId)
	{
		var application = await _context.Applications
				.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

		if (application == null)
			return false;

		_context.Applications.Remove(application);
		await _context.SaveChangesAsync();

		return true;
	}

	public async Task<ApplicationFormSchemaDto?> GetApplicationFormSchemaAsync(Guid applicationId, Guid userId)
	{
		var application = await _context.Applications
				.Include(a => a.ApplicationTemplate)
				.Include(a => a.TreeSubmission)
						.ThenInclude(ts => ts.Species)
				.Include(a => a.Municipality)
				.Include(a => a.User)
				.FirstOrDefaultAsync(a => a.Id == applicationId && a.UserId == userId);

		if (application == null)
			return null;

		// Get prefilled data from user and tree submission
		var prefilledData = GetPrefilledData(application.User, application.TreeSubmission, application.Municipality);

		// Merge with existing form data
		foreach (var item in application.FormData)
		{
			prefilledData[item.Key] = item.Value;
		}

		// Find missing required fields
		var requiredFields = application.ApplicationTemplate.Fields
				.Where(f => !prefilledData.ContainsKey(f.Name))
				.OrderBy(f => f.Order)
				.ToList();

		return new ApplicationFormSchemaDto
		{
			ApplicationId = application.Id,
			ApplicationTemplateId = application.ApplicationTemplateId,
			TemplateName = application.ApplicationTemplate.Name,
			RequiredFields = requiredFields,
			PrefilledData = prefilledData
		};
	}

	private Dictionary<string, object> GetPrefilledData(User user, TreeSubmission treeSubmission, Municipality municipality)
	{
		return new Dictionary<string, object>
		{
			["user_first_name"] = user.FirstName,
			["user_last_name"] = user.LastName,
			["user_full_name"] = user.FullName,
			["user_email"] = user.Email,
			["user_phone"] = user.Phone ?? "",
			["user_address"] = user.Address ?? "",
			["user_city"] = user.City ?? "",
			["user_postal_code"] = user.PostalCode ?? "",
			["tree_species_polish"] = treeSubmission.Species.PolishName,
			["tree_species_latin"] = treeSubmission.Species.LatinName,
			["tree_circumference"] = treeSubmission.Circumference,
			["tree_height"] = treeSubmission.Height ?? 0,
			["tree_condition"] = treeSubmission.Condition,
			["tree_estimated_age"] = treeSubmission.EstimatedAge ?? 0,
			["tree_is_alive"] = treeSubmission.IsAlive,
			["tree_is_monument"] = treeSubmission.IsMonument,
			["tree_location_latitude"] = treeSubmission.Location.Lat,
			["tree_location_longitude"] = treeSubmission.Location.Lng,
			["tree_description"] = treeSubmission.Description ?? "",
			["submission_date"] = treeSubmission.SubmissionDate.ToString("dd.MM.yyyy"),
			["municipality_name"] = municipality.Name,
			["municipality_address"] = municipality.Address,
			["municipality_city"] = municipality.City,
			["municipality_province"] = municipality.Province,
			["municipality_postal_code"] = municipality.PostalCode ?? "",
			["municipality_phone"] = municipality.Phone ?? "",
			["municipality_email"] = municipality.Email ?? "",
		};
	}

	public async Task<ApplicationDto?> SubmitApplicationAsync(Guid id, Guid userId, SubmitApplicationDto submitDto)
	{
		var application = await _context.Applications
				.Include(a => a.ApplicationTemplate)
				.Include(a => a.TreeSubmission)
						.ThenInclude(ts => ts.Species)
				.Include(a => a.User)
				.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

		if (application == null)
			return null;

		if (application.Status != ApplicationStatus.Draft)
			throw new InvalidOperationException("Można przesłać tylko wnioski w stanie roboczym");

		// Validate required fields
		var validationErrors = ValidateFormData(application.ApplicationTemplate.Fields, submitDto.FormData);
		if (validationErrors.Any())
			throw new ArgumentException($"Błędy walidacji: {string.Join(", ", validationErrors)}");

		// Update form data
		application.FormData = submitDto.FormData;
		application.Status = ApplicationStatus.Submitted;
		application.SubmittedDate = DateTime.UtcNow;

		// Generate HTML content
		var prefilledData = GetPrefilledData(application.User, application.TreeSubmission, application.Municipality);
		var allData = new Dictionary<string, object>(prefilledData);
		foreach (var item in submitDto.FormData)
		{
			allData[item.Key] = item.Value;
		}

		application.GeneratedHtmlContent = await _fileGenerationService.GenerateHtmlFromTemplateAsync(
				application.ApplicationTemplate.HtmlTemplate, allData);

		await _context.SaveChangesAsync();

		return application.MapToDto();
	}

	private List<string> ValidateFormData(List<ApplicationField> fields, Dictionary<string, object> allFormData)
	{
		var errors = new List<string>();

		foreach (var field in fields.Where(f => f.IsRequired))
		{
			if (!allFormData.ContainsKey(field.Name) ||
					allFormData[field.Name] == null ||
					string.IsNullOrWhiteSpace(allFormData[field.Name].ToString()))
			{
				errors.Add($"Pole '{field.Label}' jest wymagane");
				continue;
			}

			// Additional field-specific validations
			var value = allFormData[field.Name].ToString();

			if (value == null) continue;

			if (field.Validation != null)
			{
				// Length validation
				if (field.Validation.MinLength.HasValue && value.Length < field.Validation.MinLength.Value)
				{
					errors.Add($"Pole '{field.Label}' musi mieć co najmniej {field.Validation.MinLength.Value} znaków");
				}

				if (field.Validation.MaxLength.HasValue && value.Length > field.Validation.MaxLength.Value)
				{
					errors.Add($"Pole '{field.Label}' może mieć maksymalnie {field.Validation.MaxLength.Value} znaków");
				}

				// Pattern validation
				if (!string.IsNullOrEmpty(field.Validation.Pattern))
				{
					var regex = new System.Text.RegularExpressions.Regex(field.Validation.Pattern);
					if (!regex.IsMatch(value))
					{
						var message = !string.IsNullOrEmpty(field.Validation.ValidationMessage)
								? field.Validation.ValidationMessage
								: $"Pole '{field.Label}' ma nieprawidłowy format";
						errors.Add(message);
					}
				}

				// Numeric validation
				if (field.Type == ApplicationFieldType.Number && double.TryParse(value, out var numValue))
				{
					if (field.Validation.Min.HasValue && numValue < field.Validation.Min.Value)
					{
						errors.Add($"Pole '{field.Label}' musi być większe lub równe {field.Validation.Min.Value}");
					}

					if (field.Validation.Max.HasValue && numValue > field.Validation.Max.Value)
					{
						errors.Add($"Pole '{field.Label}' musi być mniejsze lub równe {field.Validation.Max.Value}");
					}
				}
				else if (field.Type == ApplicationFieldType.Number && !double.TryParse(value, out _))
				{
					errors.Add($"Pole '{field.Label}' musi być liczbą");
				}
			}

			// Type-specific validations
			switch (field.Type)
			{
				case ApplicationFieldType.Email:
					if (!IsValidEmail(value))
					{
						errors.Add($"Pole '{field.Label}' musi zawierać prawidłowy adres email");
					}
					break;

				case ApplicationFieldType.Phone:
					if (!IsValidPhone(value))
					{
						errors.Add($"Pole '{field.Label}' musi zawierać prawidłowy numer telefonu");
					}
					break;

				case ApplicationFieldType.Date:
					if (!DateTime.TryParse(value, out _))
					{
						errors.Add($"Pole '{field.Label}' musi zawierać prawidłową datę");
					}
					break;

				case ApplicationFieldType.DateTime:
					if (!DateTime.TryParse(value, out _))
					{
						errors.Add($"Pole '{field.Label}' musi zawierać prawidłową datę i czas");
					}
					break;
			}
		}

		return errors;
	}

	private bool IsValidEmail(string email)
	{
		try
		{
			var addr = new System.Net.Mail.MailAddress(email);
			return addr.Address == email;
		}
		catch
		{
			return false;
		}
	}

	private bool IsValidPhone(string phone)
	{
		// Simple phone validation - can be enhanced based on requirements
		var cleanPhone = phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
		return System.Text.RegularExpressions.Regex.IsMatch(cleanPhone, @"^\+?[0-9]{9,15}$");
	}

	public async Task<string> GeneratePdfFromAplicationAsync(Guid applicationId, Guid userId)
	{
		var application = await _context.Applications
		.FirstOrDefaultAsync(a => a.Id == applicationId && a.UserId == userId);

		if (application == null)
			throw new ArgumentException("Wniosek nie został znaleziony");

		if (application.Status == ApplicationStatus.Draft)
			throw new InvalidOperationException("Nie można wygenerować PDF dla wniosku w stanie roboczym");

		if (string.IsNullOrEmpty(application.GeneratedHtmlContent))
			throw new InvalidOperationException("Brak wygenerowanej treści HTML");

		// Generate PDF
		var pdfPath = await _fileGenerationService.GeneratePdfAsync(application.GeneratedHtmlContent, $"wniosek_{application.Id}.pdf");

		// Update application with PDF path
		application.GeneratedPdfPath = pdfPath;
		await _context.SaveChangesAsync();

		return pdfPath;
	}
}