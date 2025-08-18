using System;
using DrzewaAPI.Models.Enums;

namespace DrzewaAPI.Dtos.Auth;

public record UserDto
{
	public Guid Id { get; init; }
	public required string Email { get; init; }
	public required string Name { get; init; }
	public string? Avatar { get; set; }
	public DateTime RegistrationDate { get; init; }
	public int SubmissionsCount { get; init; }
	public int VerificationsCount { get; init; }
}
