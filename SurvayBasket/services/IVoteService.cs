namespace SurvayBasket.services

{
    public interface IVoteService
    {

        Task<Result> AddVote(int pollid,VoteRequest voteRequest ,  CancellationToken cancellationToken = default);
        Task<Result<PollVotesResponse>> GetVoteResults(int pollid,CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<VoteResultsResponsePerDay>>> GetVoteResultsPerDay(int pollid,CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<VotePerQuestion>>> GetVoteResultsPerQuestion(int pollid,CancellationToken cancellationToken = default);



    }
}
