using Microsoft.AspNetCore.Authorization;
using SurvayBasket.Abstractions.Consts.cs;

namespace SurvayBasket.Filter.cs
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var user = context.User.Identity;

            if (user is null || !user.IsAuthenticated)
                return;

            var hasPermission = context.User.Claims.Any(x => x.Value == requirement.Permission && x.Type == Permissions.Type); // claims --> دي الي مع التوكن ---> key(Type) - value

            /* if we use --->>> string.Join(",", Roles) */

            //        var hasPermission = context.User.Claims
            //.Where(x => x.Type == "permissions") // بنحصل على الصلاحيات فقط
            //.SelectMany(x => x.Value.Split(',')) // بنحول الـ string إلى قائمة
            //.Any(p => p.Trim() == requirement.Permission); // بنتأكد إن الصلاحية موجودة


            if (!hasPermission)
                return;

            context.Succeed(requirement);

        }
    }
}
