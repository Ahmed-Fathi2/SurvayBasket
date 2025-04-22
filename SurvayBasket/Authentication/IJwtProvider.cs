namespace SurvayBasket.Authentication
{
    public interface IJwtProvider
    {
        (string token, int expiresIn) GenerateToken(AppUser user, IEnumerable<string> Roles, IEnumerable<string> Permissions);


    }
}
