namespace SurvayBasket.Contracts.Polls

{
    #region test
    //RuleFor(x => x.DataOfBirth)
    //   .Must(x => DateTime.Now > x!.Value.AddYears(18)) // custom validation 
    //   .When(x => x.DataOfBirth.HasValue) // if this condition is not achive , this validation for this prop will not happen
    //   .WithMessage("must be > 18 years old"); 
    #endregion
    public class RequestPollValidator : AbstractValidator<PollRequest>
    {

        public RequestPollValidator()
        {
            RuleFor(x => x.Title)
                 .NotEmpty()
                 .Length(3, 100);

            RuleFor(x => x.Summary)
                .NotEmpty()
                .Length(3, 1500);

            RuleFor(x => x.StartsAt)
             .NotEmpty()
             .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

            RuleFor(x => x.EndsAt)
                .NotEmpty();

            RuleFor(x => x)
                .Must(IsValidEndDate)
                .WithName(nameof(PollRequest.EndsAt))
                .WithMessage("{PropertyName} must be greater then starts date");

        }

        private static bool IsValidEndDate(PollRequest request)
        {
            return request.EndsAt >= request.StartsAt;
        }
    }
}
