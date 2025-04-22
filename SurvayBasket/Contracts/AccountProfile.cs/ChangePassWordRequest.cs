namespace SurvayBasket.Contracts.AccountProfile.cs
{
    public record ChangePassWordRequest
    (
                
        string CurrentPassword,
        string NewPassword

    );
}
