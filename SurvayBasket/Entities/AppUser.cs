using Microsoft.AspNetCore.Identity;

namespace SurvayBasket.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } 
    }
}
