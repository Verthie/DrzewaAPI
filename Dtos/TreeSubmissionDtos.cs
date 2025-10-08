using System;
using System.ComponentModel.DataAnnotations;
using DrzewaAPI.Models.Enums;

namespace DrzewaAPI.Dtos;

public record TreeSubmissionDto
{
	public Guid Id { get; init; }
	public required UserDataDto UserData { get; init; }
	public string? Name { get; init; }
	public required string Species { get; init; }
	public required string SpeciesLatin { get; init; }
	public required LocationDto Location { get; init; }
	public required double Circumference { get; init; }
	public double Height { get; init; }
	public List<string>? Soil { get; set; }
	public List<string>? Health { get; set; }
	public List<string>? Environment { get; set; }
	public bool IsAlive { get; init; } = true;
	public int EstimatedAge { get; init; }
	public double CrownSpread { get; init; }
	public string? Description { get; init; }
	public string? Legend { get; init; }
	public List<string> ImageUrls { get; init; } = new();
	public bool IsMonument { get; init; } = false;
	public string? TreeScreenshotUrl { get; init; }
	public SubmissionStatus Status { get; init; }
	public DateTime SubmissionDate { get; init; }
	public DateTime? ApprovalDate { get; init; }
	public required int VotesCount { get; init; }
}

public record CreateTreeSubmissionDto
{
	[Required(ErrorMessage = "Gatunek jest wymagany")]
	public required Guid SpeciesId { get; init; }
	public string? Name { get; init; }
	[Required(ErrorMessage = "Lokalizacja jest wymagana")]
	public required LocationDto Location { get; init; }
	[Required(ErrorMessage = "Pierśnica jest wymagana")]
	[Range(0, double.MaxValue)]
	public required double Circumference { get; init; }
	[Required(ErrorMessage = "Wysokość jest wymagana")]
	[Range(0, double.MaxValue)]
	public required double Height { get; init; }
	public List<string>? Soil { get; init; }
	public List<string>? Health { get; init; }
	public List<string>? Environment { get; init; }
	public bool IsAlive { get; init; } = true;
	[Required(ErrorMessage = "Wiek jest wymagany")]
	[Range(0, int.MaxValue)]
	public required int EstimatedAge { get; init; }
	[Required(ErrorMessage = "Rozpiętość korony jest wymagana")]
	[Range(0, double.MaxValue)]
	public required double CrownSpread { get; init; }
	public string? Description { get; init; }
	public string? Legend { get; init; }
	[Required(ErrorMessage = "Przynajmniej jeden załącznik jest wymagany")]
	public bool IsMonument { get; init; } = false;
}

public record UpdateTreeSubmissionDto
{
	public Guid? SpeciesId { get; init; }
	public string? Name { get; init; }
	public LocationDto Location { get; init; } = new LocationDto();
	public double? Circumference { get; init; }
	public double? Height { get; init; }
	public List<string>? Soil { get; init; }
	public List<string>? Health { get; init; }
	public List<string>? Environment { get; init; }
	public bool? IsAlive { get; init; }
	public int? EstimatedAge { get; init; }
	public double? CrownSpread { get; init; }
	public string? Description { get; init; }
	public string? Legend { get; init; }
	public bool? IsMonument { get; init; }
	public string? TreeScreenshotUrl { get; init; }
	public bool? ReplaceImages { get; init; } = true; // If true, replace all images; if false, append
}

public record LocationDto
{
	public double Lat { get; init; } = 0;
	public double Lng { get; init; } = 0;
	public string? Address { get; set; }
	public string? PlotNumber { get; set; }
	public string? District { get; set; }
	public string? Province { get; set; }
	public string? County { get; set; }
	public string? Commune { get; set; }
}