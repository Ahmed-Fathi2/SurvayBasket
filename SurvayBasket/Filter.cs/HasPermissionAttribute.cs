using Microsoft.AspNetCore.Authorization;

namespace SurvayBasket.Filter.cs
{
    public class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission)
    {
    }
}
