namespace SurvayBasket.Contracts.Results.VoteResults
{
    public record VoteResponse(
    string VoterName,
    DateTime VoteDate,
    IEnumerable<AnswerOfQuestions> SelectedAnswers
);
}
