namespace SurvayBasket.Data.EntitiesConfigurations

{
    public class PollConfiguration : IEntityTypeConfiguration<Poll>
    {
        public void Configure(EntityTypeBuilder<Poll> builder)
        {

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(x => x.Title).IsUnique();

            builder.Property(x => x.Summary)
                .HasMaxLength(1500);
                




        }
    }
}
