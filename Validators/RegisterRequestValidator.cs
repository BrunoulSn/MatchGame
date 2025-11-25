using FluentValidation;
using BFF_GameMatch.Services.Dtos.Auth;

namespace MyBffProject.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        }
    }
}
