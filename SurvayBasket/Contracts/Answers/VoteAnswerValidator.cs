namespace SurvayBasket.Contracts.Answers
{
    public class VoteAnswerValidator : AbstractValidator<VoteAnswers>
    {
        public VoteAnswerValidator()
        {
            RuleFor(x => x.QuestionId)
                .GreaterThan(0);

            RuleFor(x => x.AnswerId)
                .GreaterThan(0);
        }
    }
}
