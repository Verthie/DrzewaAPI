using System;
using System.ComponentModel.DataAnnotations;
using DrzewaAPI.Models.Enums;

namespace DrzewaAPI.Dtos;

public record TreeSubmissionDto
{
	public Guid Id { get; init; }
	public required UserDataDto UserData { get; set; }
	public required string Species { get; init; }
	public required string SpeciesLatin { get; init; }
	public required LocationDto Location { get; init; }
	public required int Circumference { get; init; }
	public double Height { get; set; }
	public required string Condition { get; init; }
	public bool IsAlive { get; init; } = true;
	public int EstimatedAge { get; set; }
	public string? Description { get; set; }
	public List<string> ImageUrls { get; set; } = new();
	public bool IsMonument { get; init; } = false;
	public SubmissionStatus Status { get; init; }
	public DateTime SubmissionDate { get; init; }
	public DateTime? ApprovalDate { get; set; }
	public required VotesDto Votes { get; init; }
	public required int CommentCount { get; init; }
}

public record CreateTreeSubmissionDto
{
	[Required(ErrorMessage = "Gatunek jest wymagany")]
	public required Guid SpeciesId { get; init; }
	[Required(ErrorMessage = "Lokalizacja jest wymagana")]
	public required LocationDto Location { get; init; }
	[Required(ErrorMessage = "Pierśnica jest wymagana")]
	[Range(0, int.MaxValue)]
	public required int Circumference { get; init; }
	[Required(ErrorMessage = "Wysokość jest wymagana")]
	[Range(0, int.MaxValue)]
	public required double Height { get; set; }
	[Required(ErrorMessage = "Kondycja jest wymagana")]
	public required string Condition { get; init; }
	public bool IsAlive { get; init; } = true;
	[Required(ErrorMessage = "Wiek jest wymagany")]
	[Range(0, int.MaxValue)]
	public required int EstimatedAge { get; set; }
	public string? Description { get; set; }
	[Required(ErrorMessage = "Przynajmniej jeden załącznik jest wymagany")]
	public bool IsMonument { get; init; } = false;
}

public class UpdateTreeSubmissionDto
{
	public Guid? SpeciesId { get; set; }
	public LocationDto? Location { get; set; }
	public int? Circumference { get; set; }
	public double? Height { get; set; }
	public string? Condition { get; set; }
	public bool? IsAlive { get; set; }
	public int? EstimatedAge { get; set; }
	public string? Description { get; set; }
	public bool? IsMonument { get; set; }
	public bool? ReplaceImages { get; set; } = false; // If true, replace all images; if false, append
}

public record VoteRequestDto
{
	public VoteType Type { get; set; }
}

public record LocationDto
{
	public required double Lat { get; set; }
	public required double Lng { get; set; }
	public required string Address { get; set; }
}