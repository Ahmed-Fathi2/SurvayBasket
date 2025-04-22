namespace SurvayBasket.Contracts.Questions
{
    public record QuestionRequest(
        
        string Content , 
        List<string> Answers 
        );
    
}
