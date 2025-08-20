namespace DrzewaAPI.Models.ValueObjects;

public class Location
{
	public required double Lat { get; set; }
	public required double Lng { get; set; }
	public required string Address { get; set; }
}
