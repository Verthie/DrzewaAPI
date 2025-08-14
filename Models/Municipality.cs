using System;

namespace DrzewaAPI.Models;

public class Municipality
{
	public required Guid Id { get; set; }
	public required string Name { get; set; }
	public required string Province { get; set; }
	public required string City { get; set; }
	public required string Address { get; set; }
	public required string ZipCode { get; set; }
	public required string Email { get; set; }

	// Navigation Properties
	public ICollection<Application>? Applications { get; set; }
}
