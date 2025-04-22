using SurvayBasket.Abstractions.Consts.cs;

namespace SurvayBasket.Contracts.Authentication
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {

        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(3, 200);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(3, 200);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(RegexPattern.passwordPattern)
                .MinimumLength(8);

        }
    }
}
