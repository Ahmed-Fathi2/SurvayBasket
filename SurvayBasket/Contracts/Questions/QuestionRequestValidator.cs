namespace SurvayBasket.Contracts.Questions
{
    public class QuestionRequestValidator:AbstractValidator<QuestionRequest>
    {

        public QuestionRequestValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()    // not empty and not null
                .MaximumLength(1000);


            RuleFor(x => x.Answers)
                .NotNull();  //  not null only


            RuleFor(x => x.Answers)
                .Must(x => x.Count > 1)
                .WithMessage(" Num Of Answers must be At Least 2 for Each Question ")
                .When(x => x.Answers != null);
            


            RuleFor(x => x.Answers)
                .Must(x => x.Distinct().Count() == x.Count)
                .WithMessage(" there is Dublicated Answer for the Question")
                .When(x => x.Answers != null);
            


        }
    }
}
