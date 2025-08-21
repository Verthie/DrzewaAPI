namespace DrzewaAPI.Models.ValueObjects;

public record Location
{
	public required double Lat { get; set; }
	public required double Lng { get; set; }
	public required string Address { get; set; }
}
