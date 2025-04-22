namespace SurvayBasket.Contracts.Results.VoteResultsPerDay.cs
{
    public record VoteResultsResponsePerDay
      (
        
        DateOnly SubmittingDay,
        int VotesNumer
                
      );
}
