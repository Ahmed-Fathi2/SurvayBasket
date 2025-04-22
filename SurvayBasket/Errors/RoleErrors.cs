namespace SurvayBasket.Errors
{
    public static class RoleErrors
    {
        public static readonly Error RoleNotFound = new Error(" Role Not Found ", "Role with given id dosent Exist ");
        public static readonly Error RoleIsDublicated = new Error(" Role Exist ", "Role is already Exist with the same name ");
        public static readonly Error InvalidPermissions = new Error(" InvalidPermissions ", "Permissions are invalid ");
        public static readonly Error InvalidRoles = new Error(" Invalid Roles ", "Roles are invalid ");
    }
}
