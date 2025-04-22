using SurvayBasket.Abstractions.Consts.cs;

namespace SurvayBasket.Contracts.AccountProfile.cs
{
    public class ChangePassWordRequestValidator:AbstractValidator<ChangePassWordRequest>
    {
        public ChangePassWordRequestValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
             .NotEmpty()
             .Matches(RegexPattern.passwordPattern)
             .MinimumLength(8)
             .NotEqual(x => x.CurrentPassword).WithMessage("New password cant be the same of Current PassWord");
        }
    }
}
