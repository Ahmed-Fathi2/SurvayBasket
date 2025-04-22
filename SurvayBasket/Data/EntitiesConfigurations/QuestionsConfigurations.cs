
namespace SurvayBasket.Data.EntitiesConfigurations
{
    public class QuestionsConfigurations : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {

            builder.HasIndex(x => new { x.Content, x.Pollid })
                   .IsUnique();


            builder.Property(x => x.Content)
                   .HasMaxLength(1000)
                   .IsRequired();

            //builder.HasOne(x => x.Poll)
            //    .WithMany(x => x.Questions)
            //    .HasForeignKey(x => x.Pollid);


        }
    }
}
