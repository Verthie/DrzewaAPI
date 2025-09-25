using System;
using FluentValidation;

namespace DrzewaAPI.Utils;

public class ResetPasswordValidator : AbstractValidator<UpdatePasswordDto>
{
	public ResetPasswordValidator()
	{
		RuleFor(x => x.Token)
				.NotEmpty()
				.WithMessage("Token jest wymagany");

		RuleFor(x => x.NewPassword)
				.NotEmpty()
				.WithMessage("Nowe hasło jest wymagane")
				.MinimumLength(6)
				.WithMessage("Hasło musi mieć co najmniej 6 znaków")
				.Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)")
				.WithMessage("Hasło musi zawierać co najmniej jedną małą literę, wielką literę i cyfrę");

		RuleFor(x => x.ConfirmPassword)
				.Equal(x => x.NewPassword)
				.WithMessage("Hasła muszą być identyczne");
	}
}