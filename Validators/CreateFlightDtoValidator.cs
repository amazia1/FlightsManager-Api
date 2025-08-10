
namespace API.Validators
{
    public class CreateFlightDtoValidator : AbstractValidator<CreateFlightDto>
    {
        public CreateFlightDtoValidator()
        {
             RuleFor(f => f.Destination)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Destination is required.")
                .Must(s => !string.IsNullOrWhiteSpace(s))
                    .WithMessage("Destination is required.")
                .MaximumLength(100);

            RuleFor(f => f.Gate)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Gate is required.")
                .Must(s => !string.IsNullOrWhiteSpace(s))
                    .WithMessage("Gate is required.")
                .MaximumLength(10);

            RuleFor(f => f.DepartureTime)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("DepartureTime is required.")
                .GreaterThan(_ => DateTime.Now)
                .WithMessage("DepartureTime must be in the future.");
        }
    }
}