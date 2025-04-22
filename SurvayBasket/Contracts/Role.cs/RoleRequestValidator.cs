namespace SurvayBasket.Contracts.Role.cs
{
    public class RoleRequestValidator : AbstractValidator<RoleRequest>
    {
        public RoleRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(3, 256);


            RuleFor(x => x.Permissions)
                .NotEmpty();



            RuleFor(x => x.Permissions)
                .Must(x => x.Distinct().Count() == x.Count())
                .When(x => x.Permissions != null)
                .WithMessage(" You Cant add dublicated Permission For the same role ");





        }
    }
}
