using Microsoft.AspNetCore.Authorization;

namespace SurvayBasket.Filter.cs
{
    public class PermissionRequirement(string permission) : IAuthorizationRequirement
    {
        public string Permission { get; } = permission;
    }
}
