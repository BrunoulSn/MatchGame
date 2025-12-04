using FluentValidation;
using BFF_GameMatch.Services.Dtos.Team;
namespace BFF_GameMatch.Validators
{
    public class TeamCreateDtoValidator : AbstractValidator<TeamCreateDto>
    {
        public TeamCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Description).MaximumLength(2000);
            RuleFor(x => x.SportType).MaximumLength(100);
        }
    }
}