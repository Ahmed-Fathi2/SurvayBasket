namespace SurvayBasket.Entities
{
    public class AppRole:IdentityRole
    {
        public bool IsDefault { get; set; }
        public bool IsDeleted { get; set; }
    }
}
