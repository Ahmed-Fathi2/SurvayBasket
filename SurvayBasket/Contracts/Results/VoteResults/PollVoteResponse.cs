namespace SurvayBasket.Contracts.Results.VoteResults
{
    public record PollVotesResponse(
     string Title,
     IEnumerable<VoteResponse> Votes
 );
}
