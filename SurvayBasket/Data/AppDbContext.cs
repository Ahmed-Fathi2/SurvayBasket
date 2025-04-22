
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SurvayBasket.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : IdentityDbContext<AppUser,AppRole,string>(options)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public DbSet<Poll> Polls { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<VoteAnswer> VoteAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure this line is included to call the base implementation


            var cascadedfks = modelBuilder.Model.GetEntityTypes()
                             .SelectMany(x => x.GetForeignKeys())
                             .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);
            foreach (var fk in cascadedfks)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<AuditEntitiy>();
            var UserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier); // to find user id fro token claims

            foreach (var entry in entries) 
            { 
            if(entry.State == EntityState.Added)
                {
                    entry.Property(x => x.CreatedById).CurrentValue = UserId!;

                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(x => x.UpdatedById).CurrentValue = UserId;
                    entry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;

                }

            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
