namespace DrzewaAPI.Dtos.Municipality;

public record MunicipalityDto
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
