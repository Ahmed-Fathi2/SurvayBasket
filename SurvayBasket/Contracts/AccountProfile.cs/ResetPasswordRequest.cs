namespace SurvayBasket.Contracts.AccountProfile.cs
{
    public record ResetPasswordRequest
    (
        
        string Email ,
        string Code ,
        string NewPassword 
        
        
        );
}
