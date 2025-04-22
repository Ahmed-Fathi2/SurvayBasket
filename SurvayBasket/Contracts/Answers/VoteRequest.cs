namespace SurvayBasket.Contracts.Answers
{
    public record VoteRequest
        (
            IEnumerable<VoteAnswers> answers
        );
    
}
