using System;

namespace DrzewaAPI.Models;

public class Municipality
{
	public Guid Id { get; set; }
	public required string Name { get; set; }
	public required string Province { get; set; }
	public required string City { get; set; }
	public required string Address { get; set; }
	public required string ZipCode { get; set; }

	// Navigation Properties
	public required ICollection<Application> Applications { get; set; }
}
