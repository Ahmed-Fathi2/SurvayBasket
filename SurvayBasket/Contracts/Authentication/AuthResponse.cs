namespace SurvayBasket.Contracts.Authentication
{
    public record AuthResponse(
        String Id,
        string? Email,
        string FirstName,
        string LastName,
        string Token,
        int ExpireIn
        );
    
}
