using System;

namespace DrzewaAPI.Models;

public class TreeSpeciesImages
{
	public required Guid TreeSpeciesId { get; set; }
	public required Guid ImageId { get; set; }

	// Navigation Properties
	public TreeSpecies? TreeSpecies { get; set; }
	public SpeciesImage? SpeciesImage { get; set; }
}
