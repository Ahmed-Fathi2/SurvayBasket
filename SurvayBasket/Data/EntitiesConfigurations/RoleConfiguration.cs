
using SurvayBasket.Abstractions.Consts.cs;

namespace SurvayBasket.Data.EntitiesConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.HasData([
                new AppRole{

                    Id = DefaultRole.AdminRoleId,
                    Name=DefaultRole.Admin,
                    NormalizedName=DefaultRole.Admin.ToUpper(),
                    ConcurrencyStamp=DefaultRole.AdminConcurrencyStamp,
                },

                new AppRole{

                    Id = DefaultRole.MemberRoleId,
                    Name=DefaultRole.Member,
                    NormalizedName=DefaultRole.Member.ToUpper(),
                    ConcurrencyStamp=DefaultRole.MemberConcurrencyStamp,
                    IsDefault=true,

                },


                ]);
        }
    }
}
