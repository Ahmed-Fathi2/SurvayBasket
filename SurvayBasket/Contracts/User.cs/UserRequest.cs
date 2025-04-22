namespace SurvayBasket.Contracts.User.cs
{
    public record UserRequest(

        string FirstName,
        string LastName,
        string Email,
        string Password,
        IList<string> Roles

        );
}
