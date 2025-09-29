using System;
using System.ComponentModel.DataAnnotations;

namespace DrzewaAPI.Dtos;

public record CommuneDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public string Province { get; set; } = string.Empty;
	public string PostalCode { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string? Website { get; set; }
	public bool IsActive { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime? LastModifiedDate { get; set; }
}

public record CreateCommuneDto
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


public record class UpdateCommuneDto
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
