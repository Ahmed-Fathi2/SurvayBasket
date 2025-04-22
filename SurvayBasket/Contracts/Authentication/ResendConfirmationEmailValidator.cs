namespace SurvayBasket.Contracts.Authentication
{
    public class ResendConfirmationEmailValidator:AbstractValidator<ResendConfirmationEmail>
    {
        public ResendConfirmationEmailValidator()
        {

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
                
        }
    }
}
