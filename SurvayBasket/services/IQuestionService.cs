using SurvayBasket.Contracts.Common.cs;
using SurvayBasket.Contracts.Questions;

namespace SurvayBasket.services
{
    public interface IQuestionService
    {
        Task<Result<PaginationList<QuestionResponse>>> GetAllQuestionsAsync(int pollId, RequestFilters filters, CancellationToken cancellationToken);

        Task<Result<IEnumerable<QuestionResponse>>> GetAllAvailable(int pollid, CancellationToken cancellationToken = default);

        Task<Result<QuestionResponse>> GetAsync(int pollId, int Questionid, CancellationToken cancellationToken);

        Task<Result<QuestionResponse>> AddAsync (int pollId , QuestionRequest questionrequest , CancellationToken cancellationToken );

        Task<Result> UpdateAsync(int pollId, int Questionid, QuestionRequest questionrequest, CancellationToken cancellationToken);

        Task<Result> ToggleAsync(int pollId, int Questionid, CancellationToken cancellationToken);



    }
}
