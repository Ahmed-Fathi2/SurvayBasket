﻿
namespace SurvayBasket.Data.EntitiesConfigurations
{
    public class VoteConfiguration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.HasIndex(x => new { x.PollId, x.UserId }).IsUnique();

            //builder.HasOne(x => x.Poll)
            //    .WithMany(x => x.Votes)
            //    .HasForeignKey(x => x.PollId);

            


        }
    }
}
