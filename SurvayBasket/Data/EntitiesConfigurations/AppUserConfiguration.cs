
using SurvayBasket.Abstractions.Consts.cs;

namespace SurvayBasket.Data.EntitiesConfigurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(x => x.FirstName)
                .HasMaxLength(200);

            builder.Property(x => x.LastName)
           .HasMaxLength(200);

            var Hasher = new PasswordHasher<AppUser>();
            builder.HasData(new AppUser
            {

                Id = DefaultUser.AdminId,
                FirstName = "Survay-Basket",
                LastName = "Admin",
                Email =DefaultUser.AdminEmail,
                NormalizedEmail= DefaultUser.AdminEmail.ToUpper(),
                UserName = DefaultUser.AdminEmail,
                NormalizedUserName = DefaultUser.AdminEmail.ToUpper(),
                ConcurrencyStamp = DefaultUser.AdminConcurrencyStamp,
                SecurityStamp = DefaultUser.AdminSecurityStamp,
                PasswordHash = Hasher.HashPassword(null!, DefaultUser.AdminPassword),
                EmailConfirmed=true,

          
            });

        }
    }
}
