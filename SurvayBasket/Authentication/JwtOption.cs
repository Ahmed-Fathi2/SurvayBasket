using System.ComponentModel.DataAnnotations;

namespace SurvayBasket.Authentication
{
    public class Jwtoptions
    {
        [Required]
        public string Key { get; init; } = string.Empty;

        [Required]
        public string Issuer { get; init; } = string.Empty;

        [Required]
        public string Audience { get; init; } = string.Empty;

        [Range(1,int.MaxValue,ErrorMessage = "invalid Expiry Minutes")]
        public int ExpiryMinutes { get; init; }

    }
}
