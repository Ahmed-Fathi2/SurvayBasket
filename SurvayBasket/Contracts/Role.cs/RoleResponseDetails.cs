namespace SurvayBasket.Contracts.Role.cs
{
    public record RoleResponseDetails
    (

        string Id,
        string Name,
        bool IsDeleted,
        IEnumerable<string> Permissions

     );
}
