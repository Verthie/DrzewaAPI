using System;

namespace DrzewaAPI.Models;

public class TreeSpeciesImages
{
	public Guid TreeSpeciesId { get; set; }
	public Guid ImageId { get; set; }

	// Navigation Properties
	public TreeSpecies? TreeSpecies { get; set; }
	public SpeciesImage? SpeciesImage { get; set; }
}
