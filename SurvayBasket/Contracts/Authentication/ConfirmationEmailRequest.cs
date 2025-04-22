namespace SurvayBasket.Contracts.Authentication
{
    public record ConfirmationEmailRequest
     (
        
          string Id ,
          String ConfirmationToken
        
     );
}
