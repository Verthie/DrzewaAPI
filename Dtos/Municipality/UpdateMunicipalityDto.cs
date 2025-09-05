using System.ComponentModel.DataAnnotations;

namespace DrzewaAPI.Dtos.Municipality;

public record class UpdateMunicipalityDto
{
	[MaxLength(100)]
	public string? Name { get; set; }

	[MaxLength(200)]
	public string? Address { get; set; }

	[MaxLength(100)]
	public string? City { get; set; }

	[MaxLength(100)]
	public string? Province { get; set; }

	[MaxLength(10)]
	public string? PostalCode { get; set; }

	[Phone]
	[MaxLength(15)]
	public string? Phone { get; set; }

	[EmailAddress]
	[MaxLength(100)]
	public string? Email { get; set; }

	[MaxLength(100)]
	public string? Website { get; set; }

	public bool? IsActive { get; set; }
}
