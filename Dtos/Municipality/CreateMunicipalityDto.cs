using System.ComponentModel.DataAnnotations;

namespace DrzewaAPI.Dtos.Municipality;

public record CreateMunicipalityDto
{
	[Required]
	[MaxLength(100)]
	public required string Name { get; set; }

	[Required]
	[MaxLength(200)]
	public required string Address { get; set; }

	[Required]
	[MaxLength(100)]
	public required string City { get; set; }

	[Required]
	[MaxLength(100)]
	public required string Province { get; set; }

	[Required]
	[MaxLength(10)]
	public required string PostalCode { get; set; }

	[Required]
	[Phone]
	[MaxLength(15)]
	public required string Phone { get; set; }

	[Required]
	[EmailAddress]
	[MaxLength(100)]
	public required string Email { get; set; }

	[MaxLength(100)]
	public string? Website { get; set; }
}
