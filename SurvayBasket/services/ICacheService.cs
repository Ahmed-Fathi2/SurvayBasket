namespace SurvayBasket.services
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string cacheKey, CancellationToken cancellationToken) where T : class ;  
        Task  SetAsync<T>(string cacheKey,T Value  , CancellationToken cancellationToken) where T : class ;
        Task RemoveAsync(string cacheKey, CancellationToken cancellationToken); 



    }
}
