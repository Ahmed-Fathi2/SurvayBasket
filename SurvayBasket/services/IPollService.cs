
namespace SurvayBasket.services

{
    public interface IPollService
    {
        Task<Result<IEnumerable<PollResponse>>> GetAllAsync(CancellationToken cancellationToken);
        Task<Result<IEnumerable<PollResponse>>> GetAllAvailableAsync(CancellationToken cancellationToken);

        Task<Result<PollResponse>> GetAsync(int id, CancellationToken cancellationToken =default);

        Task<Result<PollResponse>> AddAsync(PollRequest Request, CancellationToken cancellationToken = default);

        Task <Result> UpdateAsync(int id , PollRequest request, CancellationToken cancellationToken = default);

        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);

        Task<Result> TogglePublishAsync(int id, CancellationToken cancellationToken = default);

    }
}
