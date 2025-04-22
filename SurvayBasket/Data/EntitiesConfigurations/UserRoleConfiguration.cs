
using SurvayBasket.Abstractions.Consts.cs;

namespace SurvayBasket.Data.EntitiesConfigurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(new IdentityUserRole<string>
            {

                UserId = DefaultUser.AdminId,
                RoleId= DefaultRole.AdminRoleId

            });

        }
    }
}
