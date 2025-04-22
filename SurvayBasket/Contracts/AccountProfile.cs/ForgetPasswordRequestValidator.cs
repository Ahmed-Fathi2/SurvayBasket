namespace SurvayBasket.Contracts.AccountProfile.cs
{
    public class ForgetPasswordRequestValidator:AbstractValidator<ForgetPasswordRequest>
    {
        public ForgetPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
