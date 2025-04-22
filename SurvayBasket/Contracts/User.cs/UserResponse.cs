namespace SurvayBasket.Contracts.User.cs
{
    public record UserResponse
    (
        string id,
        string FirstName,
        string LastName,
        string Email,
        IList<string> Roles
        
        );
}
