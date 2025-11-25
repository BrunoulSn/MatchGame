using FluentValidation;
using BFF_GameMatch.Services.Dtos.Auth;

namespace MyBffProject.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        }
    }
}
