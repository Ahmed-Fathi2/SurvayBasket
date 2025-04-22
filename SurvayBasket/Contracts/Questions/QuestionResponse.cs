using SurvayBasket.Contracts.Answers;

namespace SurvayBasket.Contracts.Questions
{
    public record QuestionResponse
        (
            int id ,
            string Content ,
            IEnumerable<AnswersResponse> Answers 

        );

}
