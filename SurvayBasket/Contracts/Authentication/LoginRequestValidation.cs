using System.Data;

namespace SurvayBasket.Contracts.Authentication
{
    public class LoginRequestValidation:AbstractValidator<LoginRequest>
    {
        public LoginRequestValidation()
        {
            RuleFor(x=>x.Email).NotEmpty();

            RuleFor(x=>x.Email).EmailAddress();

            RuleFor (x=>x.Password).NotEmpty();
        }

    }
}
