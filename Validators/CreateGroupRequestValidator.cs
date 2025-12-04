using FluentValidation;

using BFF_GameMatch.Services.Dtos.Group;



namespace BFF_GameMatch.Validators
{
    public class CreateGroupRequestValidator : AbstractValidator<CreateGroupRequest>
    {
        public CreateGroupRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).MaximumLength(2000);
        }
    }
}
