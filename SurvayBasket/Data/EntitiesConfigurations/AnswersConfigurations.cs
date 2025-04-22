
namespace SurvayBasket.Data
{
    public class AnswersConfigurations : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {

            builder.HasIndex(x => new { x.Content, x.QuestionId })
                   .IsUnique();

            builder.Property(x => x.Content)
                   .HasMaxLength(1000)
                   .IsRequired();

            //builder.HasOne(x => x.Question)
            //    .WithMany(x => x.Answers)
            //    .HasForeignKey(x => x.QuestionId);

        }
    }
}
