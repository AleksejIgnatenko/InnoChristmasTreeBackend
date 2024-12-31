using FluentValidation;
using InnoChristmasTree.Models;

namespace InnoChristmasTree.Validations
{
    public class CongratulationModelValidation : AbstractValidator<CongratulationModel>
    {
        // Валидация модели поздравления
        public CongratulationModelValidation()
        {
            RuleFor(x => x.Icon)
                .NotEmpty().WithMessage("Icon is required.")
                .Matches("^(🎄|🎁|🎅|⛄|❄️)$").WithMessage("Icon must be one of the following: 🎄, 🎁, 🎅, ⛄, ❄️");

            RuleFor(x => x.CongratulationText)
                .NotEmpty().WithMessage("Congratulation text is required.")
                .MaximumLength(128).WithMessage("Congratulation text cannot be longer than 128 characters.");
        }
    }
}
