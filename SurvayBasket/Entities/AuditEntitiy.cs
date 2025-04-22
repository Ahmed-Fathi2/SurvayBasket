namespace SurvayBasket.Entities
{
    public class AuditEntitiy
    {
        public string CreatedById { get; set; } = String.Empty;
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
        public AppUser CreatedBy { get; set; } = default!;


        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedById { get; set; }
        public AppUser? UpdatedBy { get; set; }
    }
}
