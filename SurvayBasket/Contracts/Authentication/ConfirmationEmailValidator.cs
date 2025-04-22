namespace SurvayBasket.Contracts.Authentication
{
    public class ConfirmationEmailValidator :AbstractValidator<ConfirmationEmailRequest>
    {

        public ConfirmationEmailValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();


            RuleFor(x => x.ConfirmationToken)
                .NotEmpty();

        }

    }
}
