using SurvayBasket.Contracts.User.cs;

namespace SurvayBasket.services
{
    public interface IUserService
    {

        Task<Result< IEnumerable<UserResponse>>> GetAllAsync ( CancellationToken cancellationToken = default);
        Task<Result<UserResponse>> GetAsync (string id ,  CancellationToken cancellationToken = default);
        Task<Result<UserResponse>> AddAsync (UserRequest request ,  CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync (string id , UserUpdate request ,  CancellationToken cancellationToken = default);
        Task<Result> ToggleAsync (string id ,  CancellationToken cancellationToken = default);
        Task<Result> UnLockAsync (string id ,  CancellationToken cancellationToken = default);
    }
}
