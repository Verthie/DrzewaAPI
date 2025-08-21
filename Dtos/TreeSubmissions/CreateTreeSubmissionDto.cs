using System.ComponentModel.DataAnnotations;
using DrzewaAPI.Models.Enums;
using DrzewaAPI.Models.ValueObjects;

namespace DrzewaAPI.Dtos.TreeSubmissions;

public record CreateTreeSubmissionDto
{
	public Guid Id { get; init; }
	[Required(ErrorMessage = "Gatunek jest wymagany")]
	public required Guid SpeciesId { get; init; }
	[Required(ErrorMessage = "Lokalizacja jest wymagana")]
	public required Location Location { get; init; }
	[Required(ErrorMessage = "Pierśnica jest wymagana")]
	[Range(0, int.MaxValue)]
	public required int Circumference { get; init; }
	[Range(0, int.MaxValue)]
	public double? Height { get; set; }
	[Required(ErrorMessage = "Kondycja jest wymagana")]
	public required string Condition { get; init; }
	public bool IsAlive { get; init; } = true;
	public int? EstimatedAge { get; set; }
	public string? Description { get; set; }
	[Required(ErrorMessage = "Przynajmniej jeden załącznik jest wymagany")]
	public required List<string> Images { get; set; }
	public bool IsMonument { get; init; } = false;
}
