namespace DrzewaAPI.Models.ValueObjects;

public class Location
{
	public required double Latitude { get; set; }
	public required double Longitude { get; set; }
	public required string Address { get; set; }
}
