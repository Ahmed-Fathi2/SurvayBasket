using SurvayBasket.Contracts.Role.cs;

namespace SurvayBasket.services
{
    public interface IRoleService
    {

        Task<Result<IEnumerable<RoleResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<RoleResponse>>> GetAllActiveAsync(CancellationToken cancellationToken = default);
        Task<Result<RoleResponseDetails>> GetAsync(string id ,  CancellationToken cancellationToken = default);
        Task<Result<RoleResponseDetails>> AddAsync(RoleRequest request ,  CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(string id ,RoleRequest request ,  CancellationToken cancellationToken = default);
        Task<Result> ToggleAsync(string id , CancellationToken cancellationToken = default);
    }
}
