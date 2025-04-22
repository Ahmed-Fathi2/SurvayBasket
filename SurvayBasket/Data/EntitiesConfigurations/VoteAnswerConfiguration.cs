
namespace SurvayBasket.Data.EntitiesConfigurations
{
    public class VoteAnswerConfiguration : IEntityTypeConfiguration<VoteAnswer>
    {
        public void Configure(EntityTypeBuilder<VoteAnswer> builder)
        {
            builder.HasIndex(x => new { x.VoteId, x.QuestionId }).IsUnique();


            //builder.HasOne(x => x.Vote)
            //   .WithMany(x => x.VoteAnswers)
            //   .HasForeignKey(x => x.VoteId);


            //builder.HasOne(x => x.Question)
            //  .WithMany(x => x.VoteAnswers)
            //  .HasForeignKey(x => x.QuestionId);


            //builder.HasOne(x => x.Answer)
            //  .WithMany(x => x.VoteAnswers)
            //  .HasForeignKey(x => x.AnswerId);

        }
    }
}
