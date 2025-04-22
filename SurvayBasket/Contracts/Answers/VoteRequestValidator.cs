namespace SurvayBasket.Contracts.Answers
{
    public class VoteRequestValidator:AbstractValidator<VoteRequest>
    {
        public VoteRequestValidator()
        {

            RuleFor(x => x.answers)
                .NotEmpty();
                
            RuleForEach(x => x.answers)
                .SetInheritanceValidator(v=>v.Add(new VoteAnswerValidator()));


        }
    }
}
