using DrzewaAPI.Data;
using DrzewaAPI.Extensions;
using DrzewaAPI.Models;
using DrzewaAPI.Utils;
using Microsoft.EntityFrameworkCore;

namespace DrzewaAPI.Services;

public class ApplicationService : IApplicationService
{
	private readonly ApplicationDbContext _context;
	private readonly IFileGenerationService _fileGenerationService;
	private readonly IAzureStorageService _azureStorageService;
	private readonly ILogger<ApplicationService> _logger;

	public ApplicationService(
			ApplicationDbContext context,
			IFileGenerationService fileGenerationService,
			IAzureStorageService azureStorageService,
			ILogger<ApplicationService> logger)
	{
		_context = context;
		_fileGenerationService = fileGenerationService;
		_logger = logger;
		_azureStorageService = azureStorageService;
	}

	public async Task<List<ApplicationDto>> GetUserApplicationsAsync(Guid userId)
	{
		try
		{
			List<ApplicationDto> applications = await _context.Applications
					.Include(a => a.ApplicationTemplate)
					.Include(a => a.TreeSubmission)
							.ThenInclude(ts => ts.Species)
					.Where(a => a.UserId == userId)
					.OrderByDescending(a => a.CreatedDate)
					.Select(a => a.MapToDto())
					.ToListAsync();

			return applications;
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania listy wniosków");
			throw new ServiceException($"Nie można pobrać listy wniosków", "APPLICATION_FETCH_ERROR");
		}
	}

	public async Task<ApplicationDto> GetApplicationByIdAsync(Guid id, Guid userId)
	{
		try
		{
			Application application = await _context.Applications
					.Include(a => a.ApplicationTemplate)
					.Include(a => a.TreeSubmission)
							.ThenInclude(ts => ts.Species)
					.Include(a => a.User)
					.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId)
					?? throw EntityNotFoundException.ForApplication(id);

			return application.MapToDto();
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania wniosku {ApplicationId}", id);
			throw new ServiceException($"Nie można pobrać wniosku {id}", "APPLICATION_FETCH_ERROR");
		}
	}

	public async Task<ApplicationDto> CreateApplicationAsync(Guid userId, CreateApplicationDto createDto)
	{
		try
		{
			// Verify that tree submission exists and belongs to user
			TreeSubmission treeSubmission = await _context.TreeSubmissions
					.Include(ts => ts.Species)
					.FirstOrDefaultAsync(ts => ts.Id == createDto.TreeSubmissionId && ts.UserId == userId)
					?? throw new EntityNotFoundException($"Nie znaleziono drzewa o ID {createDto.TreeSubmissionId} przypisanego do użytkownika o ID {userId}", "TREE_NOT_FOUND");

			// Verify that application template exists and is active
			ApplicationTemplate template = await _context.ApplicationTemplates
				 .FirstOrDefaultAsync(at => at.Id == createDto.ApplicationTemplateId && at.IsActive)
				 ?? throw new EntityNotFoundException($"Szablon wniosku o ID {createDto.ApplicationTemplateId} nie został znaleziony lub jest nieaktywny", "TEMPLATE_NOT_FOUND");

			Application application = new Application
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
		catch (BusinessException)
		{
			throw;
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError(ex, "Błąd bazy danych podczas tworzenia drzewa");
			throw EntityCreationFailedException.ForApplication("Błąd podczas zapisu do bazy danych");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas tworzenia drzewa");
			throw EntityCreationFailedException.ForApplication("Nieoczekiwany błąd systemu");
		}
	}

	public async Task<ApplicationDto> UpdateApplicationAsync(Guid applicationId, Guid userId, UpdateApplicationDto updateDto)
	{
		try
		{
			Application application = await _context.Applications
					.Include(a => a.ApplicationTemplate)
					.Include(a => a.TreeSubmission)
							.ThenInclude(ts => ts.Species)
					.FirstOrDefaultAsync(a => a.Id == applicationId && a.UserId == userId)
					?? throw EntityNotFoundException.ForUserApplication(applicationId, userId);

			// Can only update draft applications
			if (application.Status != ApplicationStatus.Draft)
				throw new EntityAccessDeniedException("Można edytować tylko wnioski w stanie roboczym", "ACCESS_DENIED");

			if (updateDto.FormData != null)
				application.FormData = updateDto.FormData;

			await _context.SaveChangesAsync();

			return application.MapToDto();
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError(ex, "Błąd podczas wprowadzania danych do bazy");
			throw EntityUpdateFailedException.ForApplication(userId, "Błąd podczas zapisu do bazy danych");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Nieoczekiwany błąd podczas aktualizacji danych wniosku");
			throw EntityUpdateFailedException.ForApplication(userId, "Nieoczekiwany błąd systemu");
		}
	}

	public async Task DeleteApplicationAsync(Guid applicationId, Guid userId)
	{
		try
		{
			Application application = await _context.Applications
					.FirstOrDefaultAsync(a => a.Id == applicationId && a.UserId == userId)
					?? throw EntityNotFoundException.ForUserApplication(applicationId, userId);

			_context.Applications.Remove(application);
			await _context.SaveChangesAsync();
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas usuwania wniosku {ApplicationId}", applicationId);
			throw new ServiceException($"Nie można usunąć wniosku {applicationId}", "APPLICATION_DELETE_ERROR");
		}
	}

	public async Task<ApplicationFormSchemaDto> GetApplicationFormSchemaAsync(Guid applicationId, Guid userId)
	{
		try
		{
			Application application = await _context.Applications
					.Include(a => a.ApplicationTemplate)
						.ThenInclude(at => at.Municipality)
					.Include(a => a.TreeSubmission)
							.ThenInclude(ts => ts.Species)
					.Include(a => a.User)
					.FirstOrDefaultAsync(a => a.Id == applicationId && a.UserId == userId)
					?? throw EntityNotFoundException.ForUserApplication(applicationId, userId);

			// Get prefilled data from user, tree submission and municipality
			Dictionary<string, object> prefilledData = GetPrefilledData(application);

			List<ApplicationField> requiredFields = GetRequiredFields(application, prefilledData);

			return new ApplicationFormSchemaDto
			{
				ApplicationId = application.Id,
				ApplicationTemplateId = application.ApplicationTemplateId,
				TemplateName = application.ApplicationTemplate.Name,
				RequiredFields = requiredFields,
				PrefilledData = prefilledData
			};
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas pobierania schematu wniosku");
			throw new ServiceException($"Nie można pobrać schematu wniosku", "SCHEMA_FETCH_ERROR");
		}
	}

	public async Task<ApplicationDto> SubmitApplicationAsync(Guid id, Guid userId, SubmitApplicationDto submitDto)
	{
		try
		{
			Application application = await _context.Applications
					.Include(a => a.ApplicationTemplate)
						.ThenInclude(at => at.Municipality)
					.Include(a => a.TreeSubmission)
							.ThenInclude(ts => ts.Species)
					.Include(a => a.User)
					.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId)
					?? throw EntityNotFoundException.ForApplication(id);

			List<ApplicationField> requiredFields = GetRequiredFields(application);

			if (application.Status != ApplicationStatus.Draft)
				throw new ServiceException("Można przesłać tylko wnioski w stanie roboczym", "APPLICATION_SUBMIT_ERROR");

			// Validate required fields
			Dictionary<string, List<string>> validationErrors = ValidateFormData(requiredFields, submitDto.FormData);
			if (validationErrors.Count > 0)
			{
				Dictionary<string, string[]> errorDict = validationErrors.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToArray());
				throw new Middleware.Exceptions.ValidationException(errorDict);
			}

			// Update form data
			application.FormData = submitDto.FormData;
			application.Status = ApplicationStatus.Submitted;
			application.SubmittedDate = DateTime.UtcNow;

			// Generate HTML content
			var prefilledData = GetPrefilledData(application);
			var allData = new Dictionary<string, object>(prefilledData);
			foreach (var kvp in submitDto.FormData)
			{
				allData[kvp.Key] = kvp.Value;
			}

			application.GeneratedHtmlContent = await _fileGenerationService.GenerateHtmlFromTemplateAsync(
					application.ApplicationTemplate.HtmlTemplate, allData);

			await _context.SaveChangesAsync();

			return application.MapToDto();
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError(ex, "Błąd podczas wprowadzania danych do bazy");
			throw EntityUpdateFailedException.ForApplication(userId, "Błąd podczas zapisu do bazy danych");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas składania wniosku");
			throw new ServiceException($"Nie można złożyć wniosku", "APPLICATION_SUBMIT_ERROR");
		}
	}

	public async Task<string> GeneratePdfFromAplicationAsync(Guid applicationId, Guid userId)
	{
		try
		{
			Application application = await _context.Applications
			.Include(a => a.TreeSubmission)
			.FirstOrDefaultAsync(a => a.Id == applicationId && a.UserId == userId)
			?? throw EntityNotFoundException.ForApplication(applicationId);

			if (application.Status == ApplicationStatus.Draft)
				throw new EntityAccessDeniedException("Nie można wygenerować PDF dla wniosku w stanie roboczym", "ACCESS_DENIED");

			if (string.IsNullOrEmpty(application.GeneratedHtmlContent))
				throw new ServiceException("Brak wygenerowanej treści HTML", "HTML_CONTENT_MISSING");

			// Generate PDF
			string folderPath = $"pdfs/tree-submissions/{application.TreeSubmissionId}";
			string pdfPath = await _fileGenerationService.GeneratePdfAsync(application.GeneratedHtmlContent, folderPath);

			// Update application with PDF path
			application.GeneratedPdfPath = pdfPath;
			await _context.SaveChangesAsync();

			string fileUrl = FileHelper.GetFileUrl(pdfPath, _azureStorageService);

			return fileUrl;
		}
		catch (BusinessException)
		{
			throw;
		}
		catch (DbUpdateException ex)
		{
			_logger.LogError(ex, "Błąd podczas wprowadzania danych do bazy");
			throw EntityUpdateFailedException.ForApplication(userId, "Błąd podczas zapisu do bazy danych");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Błąd podczas składania wniosku");
			throw new ServiceException($"Nie można złożyć wniosku", "APPLICATION_SUBMIT_ERROR");
		}
	}

	private List<ApplicationField> GetRequiredFields(Application application, Dictionary<string, object>? prefilledData = null)
	{
		prefilledData ??= GetPrefilledData(application);

		// Merge with existing form data
		foreach (var kvp in application.FormData)
		{
			prefilledData[kvp.Key] = kvp.Value;
		}

		// Find missing required fields
		List<ApplicationField> requiredFields = new List<ApplicationField>();

		foreach (var kvp in prefilledData)
		{
			if (kvp.Value.ToString() == "")
			{
				requiredFieldData[kvp.Key].Order = requiredFields.Count + 1;
				requiredFields.Add(
					requiredFieldData[kvp.Key]
				);
			}
		}

		List<ApplicationField> additionalTemplateFields = application.ApplicationTemplate.Fields
				.Where(f => !prefilledData.ContainsKey(f.Name))
				.OrderBy(f => f.Order)
				.ToList();

		foreach (ApplicationField field in additionalTemplateFields)
		{
			field.Order = requiredFields.Count + 1;
			requiredFields.Add(field);
		}

		return requiredFields;
	}

	private Dictionary<string, object> GetPrefilledData(Application application)
	{
		User user = application.User;
		TreeSubmission treeSubmission = application.TreeSubmission;
		Municipality municipality = application.ApplicationTemplate.Municipality;

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
			["geographic_location_lat"] = treeSubmission.Location.Lat,
			["geographic_location_long"] = treeSubmission.Location.Lng,
			["geographic_location_address"] = treeSubmission.Location.Address,
			["tree_circumference"] = treeSubmission.Circumference,
			["tree_height"] = treeSubmission.Height,
			["tree_condition"] = treeSubmission.Condition,
			["tree_estimated_age"] = treeSubmission.EstimatedAge,
			["tree_is_alive"] = treeSubmission.IsAlive,
			["tree_location_latitude"] = treeSubmission.Location.Lat,
			["tree_location_longitude"] = treeSubmission.Location.Lng,
			["tree_description"] = treeSubmission.Description ?? "",
			["submission_date"] = treeSubmission.SubmissionDate.ToString("dd.MM.yyyy"),
			["municipality_name"] = municipality.Name,
			["municipality_address"] = municipality.Address,
			["municipality_city"] = municipality.City,
			["municipality_province"] = municipality.Province,
			["municipality_postal_code"] = municipality.PostalCode,
			["municipality_phone"] = municipality.Phone,
			["municipality_email"] = municipality.Email,
		};
	}

	private Dictionary<string, List<string>> ValidateFormData(List<ApplicationField> fields, Dictionary<string, object> allFormData)
	{
		var errors = new Dictionary<string, List<string>>();

		foreach (var field in fields.Where(f => f.IsRequired))
		{
			if (!allFormData.ContainsKey(field.Name) || allFormData[field.Name] == null || string.IsNullOrWhiteSpace(allFormData[field.Name].ToString()))
			{
				if (!errors.ContainsKey(field.Name))
					errors[field.Name] = new List<string>();
				errors[field.Name].Add($"Pole '{field.Label}' jest wymagane");
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
					if (!errors.ContainsKey(field.Name))
						errors[field.Name] = new List<string>();
					errors[field.Name].Add($"Pole '{field.Label}' musi mieć co najmniej {field.Validation.MinLength.Value} znaków");
				}

				if (field.Validation.MaxLength.HasValue && value.Length > field.Validation.MaxLength.Value)
				{
					if (!errors.ContainsKey(field.Name))
						errors[field.Name] = new List<string>();
					errors[field.Name].Add($"Pole '{field.Label}' może mieć maksymalnie {field.Validation.MaxLength.Value} znaków");
				}

				// Pattern validation
				if (!string.IsNullOrEmpty(field.Validation.Pattern))
				{
					var regex = new System.Text.RegularExpressions.Regex(field.Validation.Pattern);
					if (!regex.IsMatch(value))
					{
						if (!errors.ContainsKey(field.Name))
							errors[field.Name] = new List<string>();
						var message = !string.IsNullOrEmpty(field.Validation.ValidationMessage)
								? field.Validation.ValidationMessage
								: $"Pole '{field.Label}' ma nieprawidłowy format";
						errors[field.Name].Add(message);
					}
				}

				// Numeric validation
				if (field.Type == ApplicationFieldType.Number && double.TryParse(value, out var numValue))
				{
					if (field.Validation.Min.HasValue && numValue < field.Validation.Min.Value)
					{
						if (!errors.ContainsKey(field.Name))
							errors[field.Name] = new List<string>();
						errors[field.Name].Add($"Pole '{field.Label}' musi być większe lub równe {field.Validation.Min.Value}");
					}

					if (field.Validation.Max.HasValue && numValue > field.Validation.Max.Value)
					{
						if (!errors.ContainsKey(field.Name))
							errors[field.Name] = new List<string>();
						errors[field.Name].Add($"Pole '{field.Label}' musi być mniejsze lub równe {field.Validation.Max.Value}");
					}
				}
				else if (field.Type == ApplicationFieldType.Number && !double.TryParse(value, out _))
				{
					if (!errors.ContainsKey(field.Name))
						errors[field.Name] = new List<string>();
					errors[field.Name].Add($"Pole '{field.Label}' musi być liczbą");
				}
			}

			// Type-specific validations
			switch (field.Type)
			{
				case ApplicationFieldType.Email:
					if (!IsValidEmail(value))
					{
						if (!errors.ContainsKey(field.Name))
							errors[field.Name] = new List<string>();
						errors[field.Name].Add($"Pole '{field.Label}' musi zawierać prawidłowy adres email");
					}
					break;

				case ApplicationFieldType.Phone:
					if (!IsValidPhone(value))
					{
						if (!errors.ContainsKey(field.Name))
							errors[field.Name] = new List<string>();
						errors[field.Name].Add($"Pole '{field.Label}' musi zawierać prawidłowy numer telefonu");
					}
					break;

				case ApplicationFieldType.Date:
					if (!DateTime.TryParse(value, out _))
					{
						if (!errors.ContainsKey(field.Name))
							errors[field.Name] = new List<string>();
						errors[field.Name].Add($"Pole '{field.Label}' musi zawierać prawidłową datę");
					}
					break;

				case ApplicationFieldType.DateTime:
					if (!DateTime.TryParse(value, out _))
					{
						if (!errors.ContainsKey(field.Name))
							errors[field.Name] = new List<string>();
						errors[field.Name].Add($"Pole '{field.Label}' musi zawierać prawidłową datę i czas");
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
		var cleanPhone = phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
		return System.Text.RegularExpressions.Regex.IsMatch(cleanPhone, @"^\+?[0-9]{9,15}$");
	}

	private readonly Dictionary<string, ApplicationField> requiredFieldData = new Dictionary<string, ApplicationField>
	{
		{
		"user_phone", new ApplicationField {
				Name = "user_phone",
				Label = "Numer telefonu",
				Type = ApplicationFieldType.Phone,
				IsRequired = true,
				Placeholder = "+48 123 456 789",
				Validation = new ApplicationFieldValidation
				{
						MinLength = 9,
						MaxLength = 15,
						Pattern = @"^\+?[0-9\s\-\(\)]{9,15}$",
						ValidationMessage = "Numer telefonu musi zawierać 9-15 cyfr"
				},
				HelpText = "Podaj numer telefonu kontaktowego",
				Order = 1
		}},
		{
		"user_address", new ApplicationField {
				Name = "user_address",
				Label = "Adres zamieszkania",
				Type = ApplicationFieldType.Text,
				IsRequired = true,
				Placeholder = "ul. Wiśniewska 34/1",
				Validation = new ApplicationFieldValidation
				{
						MinLength = 5,
						MaxLength = 150,
						ValidationMessage = "Adres musi mieć od 5 do 150 znaków"
				},
				HelpText = "Podaj pełny adres zamieszkania",
				Order = 2
		}},
		{
		"user_city", new ApplicationField {
				Name = "user_city",
				Label = "Miasto",
				Type = ApplicationFieldType.Text,
				IsRequired = true,
				Placeholder = "Warszawa",
				Validation = new ApplicationFieldValidation
				{
						MinLength = 2,
						MaxLength = 50,
						ValidationMessage = "Nazwa miasta musi mieć od 2 do 50 znaków"
				},
				HelpText = "Podaj nazwę miasta",
				Order = 3
		}},
		{
		"user_postal_code", new ApplicationField {
				Name = "user_postal_code",
				Label = "Kod pocztowy",
				Type = ApplicationFieldType.Text,
				IsRequired = true,
				Placeholder = "12-345",
				Validation = new ApplicationFieldValidation
				{
						Pattern = @"^\d{2}-\d{3}$",
						ValidationMessage = "Kod pocztowy musi być w formacie XX-XXX"
				},
				HelpText = "Podaj kod pocztowy w formacie XX-XXX",
				Order = 4
		}},
		{
		"tree_description", new ApplicationField {
				Name = "tree_description",
				Label = "Opis drzewa",
				Type = ApplicationFieldType.TextArea,
				IsRequired = true,
				Placeholder = "Opisz gatunek drzewa, jego wygląd, wymiary...",
				HelpText = "Podaj szczegółowy opis drzewa (gatunek, wymiary, stan)",
				Order = 5
		}}
	};
}