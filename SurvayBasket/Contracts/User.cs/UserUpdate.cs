namespace SurvayBasket.Contracts.User.cs
{
    public record UserUpdate
    (
        string FirstName,
        string LastName,
        string Email,
        IList<string> Roles

        );
}
