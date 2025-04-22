

namespace SurvayBasket.services

{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> GetTokenAsync(string Email, string Password ,CancellationToken cancellationToken=default);
        Task<Result> RegisterAsync(RegisterRequest registerRequest,CancellationToken cancellationToken=default);
        Task<Result> ConfirmEmailAsync(ConfirmationEmailRequest ConfirmEmailRequest, CancellationToken cancellationToken=default);
        Task<Result> ResendConfirmEmailAsync(ResendConfirmationEmail ResendConfirmationEmailRequest, CancellationToken cancellationToken=default);

    }
}
