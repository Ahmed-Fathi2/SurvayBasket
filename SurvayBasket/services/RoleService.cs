using NuGet.Versioning;
using StackExchange.Redis;
using SurvayBasket.Abstractions.Consts.cs;
using SurvayBasket.Contracts.Role.cs;

namespace SurvayBasket.services
{
    public class RoleService(RoleManager<AppRole> roleManager  , AppDbContext context) : IRoleService
    {
        private readonly RoleManager<AppRole> roleManager = roleManager;
        private readonly AppDbContext context = context;

        public async Task<Result<IEnumerable<RoleResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var Roles = await roleManager.Roles
                                .Where(x=>!x.IsDefault)
                                .ProjectToType<RoleResponse>()
                                .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<RoleResponse>>(Roles);


        }

        public async Task<Result<IEnumerable<RoleResponse>>> GetAllActiveAsync(CancellationToken cancellationToken = default)
        {
            var Roles = await roleManager.Roles
                                .Where(x => (!x.IsDefault && x.IsDeleted) )
                                .ProjectToType<RoleResponse>()
                                .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<RoleResponse>>(Roles);


        }

        public async Task<Result<RoleResponseDetails>> GetAsync(string id, CancellationToken cancellationToken = default)
        {
           var role = await roleManager.FindByIdAsync(id);
            if (role == null)
                return Result.Falire<RoleResponseDetails>(RoleErrors.RoleNotFound);

            var permissions = await roleManager.GetClaimsAsync(role); //------------>>>>>

            return Result.Success(new RoleResponseDetails(role.Id,role.Name!, role.IsDeleted , permissions.Select(x=>x.Value)));


        }

        public async Task<Result<RoleResponseDetails>> AddAsync(RoleRequest request, CancellationToken cancellationToken = default)
        {
            var RoleNameIsExist=  await roleManager.RoleExistsAsync(request.Name);

            if (RoleNameIsExist)
                return Result.Falire<RoleResponseDetails>(RoleErrors.RoleIsDublicated);

            var allowedPermissions = Permissions.GetAllPermissions();

            if (request.Permissions.Except(allowedPermissions).Any())
                return Result.Falire<RoleResponseDetails>(RoleErrors.InvalidPermissions);

            var NewRole = new AppRole()
            {
                Name = request.Name,
                ConcurrencyStamp = Guid.NewGuid().ToString()

            };

          var result =  await roleManager.CreateAsync(NewRole); //------------>>>>>

            if (result.Succeeded)
            {
                var permission = request.Permissions.Select(x=>new IdentityRoleClaim<string> //------------>>>>>
                {
                    ClaimType = Permissions.Type,
                    ClaimValue= x,
                    RoleId = NewRole.Id


                }).ToList();

                await context.AddRangeAsync(permission);
                await context.SaveChangesAsync();

                return Result.Success(new RoleResponseDetails(NewRole.Id, NewRole.Name!, NewRole.IsDeleted, request.Permissions));
            }
            var error = result.Errors.First();
            return Result.Falire<RoleResponseDetails>(new Error(error.Code, error.Description));
        }

        public async Task<Result> UpdateAsync(string id, RoleRequest request, CancellationToken cancellationToken = default)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
                return Result.Falire<RoleResponseDetails>(RoleErrors.RoleNotFound);

           
            var DulicatedRole = await context.Roles.AnyAsync(x => x.Name == request.Name && x.Id != id);
            if (DulicatedRole)
                return Result.Falire<RoleResponseDetails>(RoleErrors.RoleIsDublicated);


            var allowedPermissions = Permissions.GetAllPermissions();

            if (request.Permissions.Except(allowedPermissions).Any())
                return Result.Falire<RoleResponseDetails>(RoleErrors.InvalidPermissions);

            role.Name = request.Name;

            var result = await roleManager.UpdateAsync(role); //------------>>>>>

            if (result.Succeeded)
            {

                var currentPermission = await context.RoleClaims
                                            .Where(x => x.RoleId == role.Id && x.ClaimType == Permissions.Type)
                                            .Select(x => x.ClaimValue)
                                            .ToListAsync();


                var newPermission = request.Permissions.Except(currentPermission)
                                           .Select(x => new IdentityRoleClaim<string>
                                           {
                                               ClaimType = Permissions.Type,
                                               ClaimValue = x,
                                               RoleId = role.Id
                                            }).ToList();



                var DeletedPermission = currentPermission.Except(request.Permissions);

                 await context.RoleClaims
                       .Where(x => x.RoleId == role.Id && DeletedPermission.Contains(x.ClaimValue)) //---->  DeletedPermission.Contains(x.ClaimValue) == where  x.ClaimValue IN (DeletedPermission)
                       .ExecuteDeleteAsync();

                await context.AddRangeAsync(newPermission);

                await context.SaveChangesAsync();

                return Result.Success();

            }

            var error = result.Errors.First();
            return Result.Falire<RoleResponseDetails>(new Error(error.Code, error.Description));

        }

        public async Task<Result> ToggleAsync(string id, CancellationToken cancellationToken = default)
        {
        
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
                return Result.Falire<RoleResponseDetails>(RoleErrors.RoleNotFound);

            role.IsDeleted = !role.IsDeleted;

            await context.SaveChangesAsync();

            return Result.Success();
        }
    }
}

