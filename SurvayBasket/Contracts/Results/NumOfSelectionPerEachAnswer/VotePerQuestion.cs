namespace SurvayBasket.Contracts.Results.NumOfSelectionPerEachAnswer
{
    public record VotePerQuestion
    (
        string QuestionContent,
        IEnumerable<VotePerAnswer> VotePerAnswers
    );
}
