namespace DrzewaAPI.Dtos.Municipality;

public record class MunicipalityDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public string Province { get; set; } = string.Empty;
	public string? PostalCode { get; set; }
	public string? Phone { get; set; }
	public string? Email { get; set; }
	public string? Website { get; set; }
	public bool IsActive { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime? LastModifiedDate { get; set; }
}
