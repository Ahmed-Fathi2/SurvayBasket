namespace SurvayBasket.Contracts.Role.cs
{
    public record RoleRequest
    (
        string Name,
        List<string> Permissions
    );
}
