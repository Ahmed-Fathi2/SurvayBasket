
using SurvayBasket.Abstractions;
using SurvayBasket.Contracts.Common.cs;
using System.Linq.Dynamic.Core;
namespace SurvayBasket.services

{
    public class QuestionService(IHttpContextAccessor httpContextAccessor, AppDbContext context ,
                                 ICacheService cacheService ,ILogger<QuestionService> logger ): IQuestionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly AppDbContext _context = context;
        private readonly ICacheService cacheService = cacheService;
        private readonly ILogger logger = logger;
        private const string  _CachePrefix = "AvailableQuestion" ;

        public async Task<Result<PaginationList<QuestionResponse>>> GetAllQuestionsAsync(int pollId, RequestFilters filters, CancellationToken cancellationToken)
        {
            var ISPollExist = await _context.Polls.FindAsync(pollId, cancellationToken);

            if (ISPollExist == null)
            {

                return Result.Falire<PaginationList<QuestionResponse>>(PollsErrors.PollNotFound);

            }


            var query = _context.Questions.Where(x => x.Pollid == pollId);


            // filtering
            if (!string.IsNullOrEmpty(filters.SearchValue))
            {
                query = query.Where(x => x.Content.Contains(filters.SearchValue));

            };

            //sorting
            if (!string.IsNullOrEmpty(filters.SortColumn))
            {
                query = query.OrderBy($"{filters.SortColumn} {filters.SortDirection}");

            };

            // .Select(x => new QuestionResponse(x.Id, x.Content, x.Answers.Select(a => new AnswerResponse(a.Id, a.Content))))
            var source = query
                  .Include(x => x.Answers)
                  .ProjectToType<QuestionResponse>()
                  .AsNoTracking();

            var response =await PaginationList<QuestionResponse>.CreateAsync(source, filters.PageNum , filters.PageSize , cancellationToken);
            

            return Result.Success(response);

        }


        public async Task<Result<IEnumerable<QuestionResponse>>> GetAllAvailable(int pollid, CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var NoUserAccesToThisPoll = await _context.Votes.AnyAsync(x => x.PollId == pollid && x.UserId == userId);

            if (NoUserAccesToThisPoll)
            {

                return Result.Falire<IEnumerable<QuestionResponse>>(UsersErrors.ISVotedBefore);

            }

            var IsAvailablePoll = await _context.Polls
                                    .AnyAsync(x => x.Id == pollid 
                                           && x.IsPublished 
                                           && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) 
                                           && x.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken);

            if (!IsAvailablePoll)
            {

                return Result.Falire<IEnumerable<QuestionResponse>>(PollsErrors.PollNotFound);

            }


            var key = $"{_CachePrefix}-{pollid}";

            var cachedQuestion = await cacheService.GetAsync<IEnumerable<QuestionResponse>>(key, cancellationToken);

            IEnumerable<QuestionResponse> questions = [];

            if (cachedQuestion == null)
            {
                logger.LogInformation("Select from Database");
                questions = await _context.Questions.Where(x => x.Pollid == pollid && x.IsActive)
                                   .Select(x => new QuestionResponse(
                                           x.Id,
                                           x.Content,
                                           x.Answers.Where(x => x.IsActive).Select(x => new AnswersResponse(x.Id, x.Content))))
                                   .AsNoTracking().ToListAsync(cancellationToken);


            await cacheService.SetAsync(key, questions, cancellationToken);
        }

            else
            {
                logger.LogInformation("Select from cache");

                questions = cachedQuestion;

            }


            return Result.Success<IEnumerable<QuestionResponse>>(questions);

        }

        public async Task<Result<QuestionResponse>> GetAsync(int pollId, int Questionid, CancellationToken cancellationToken)
        {
            var ISPollExist = await _context.Polls.FindAsync(pollId, cancellationToken);

            if (ISPollExist == null)
            {

                return Result.Falire<QuestionResponse>(PollsErrors.PollNotFound);

            }

            var IsQuestionExist = await _context.Questions.AnyAsync(x => x.Id == Questionid && x.Pollid == pollId  ); //&&x.IsActive
            if (!IsQuestionExist)
            {

                return Result.Falire<QuestionResponse>(QuestionsErrors.QuestionNotFound);

            }

            var question = await _context.Questions.Where(x => x.Id == Questionid && x.Pollid == pollId)
                                .Include(x => x.Answers)
                                .ProjectToType<QuestionResponse>()
                                .AsNoTracking()
                                .SingleOrDefaultAsync(cancellationToken);


            return Result.Success<QuestionResponse>(question!);

        }


        public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest questionrequest, CancellationToken cancellationToken)
        {
         
            var ISPollExist = await _context.Polls.FindAsync(pollId, cancellationToken);

            if (ISPollExist == null) 
            {

                return Result.Falire<QuestionResponse>(PollsErrors.PollNotFound);
                
            }

            var IsQuestionDublicatedInTheSamePoll = await _context.Questions.AnyAsync(x=>x.Content == questionrequest.Content && x.Pollid== pollId);
            if (IsQuestionDublicatedInTheSamePoll)
            {

                return Result.Falire<QuestionResponse>(QuestionsErrors.DublicatedQuestionInTheSamePoll);

            }

            var question = questionrequest.Adapt<Question>();

            question.Pollid = pollId;
                
            //foreach(var answer in questionrequest.Answers)
            //{
            //    question.Answers.Add(new Answer { Content = answer });

            //}


            await  _context.Questions.AddAsync(question,cancellationToken);
            await _context.SaveChangesAsync();


            await cacheService.RemoveAsync($"{_CachePrefix}-{pollId}", cancellationToken);

            var QuestionResponse = question.Adapt<QuestionResponse>();  

            return Result.Success(QuestionResponse);



        }


        public async Task<Result> UpdateAsync(int pollId, int Questionid, QuestionRequest questionrequest, CancellationToken cancellationToken)
        {
            var ISPollExist = await _context.Polls.FindAsync(pollId, cancellationToken);

            if (ISPollExist == null)
            {

                return Result.Falire<QuestionResponse>(PollsErrors.PollNotFound);

            }


            var DublicatedQuestion = await _context.Questions
                .AnyAsync(x=>x.Pollid ==  pollId 
                && x.Id != Questionid 
                && x.Content == questionrequest.Content,
                cancellationToken
                
            );


            if (DublicatedQuestion)
            {

                return Result.Falire<QuestionResponse>(QuestionsErrors.DublicatedQuestionInTheSamePoll);

            }

            var question = await _context.Questions.Include(x=>x.Answers)
                                .SingleOrDefaultAsync(x=>x.Pollid==pollId && x.Id == Questionid);
            if (question == null)
            {

                return Result.Falire<QuestionResponse>(QuestionsErrors.QuestionNotFound);

            }

            question.Content = questionrequest.Content;

            var CurrentAnswers = question.Answers.Select(x=>x.Content).ToList();

            var NewAnswer = questionrequest.Answers.Except(CurrentAnswers);

            foreach (var ans in NewAnswer)
            {
                 question.Answers.Add(new Answer { Content = ans });
            }


            foreach (var ans in question.Answers)
            {
                ans.IsActive = questionrequest.Answers.Contains(ans.Content);
            }
            var x = question.Answers;

            await _context.SaveChangesAsync();

            await cacheService.RemoveAsync($"{_CachePrefix}-{pollId}", cancellationToken);


            return Result.Success();
        }

        public async Task<Result> ToggleAsync(int pollId, int Questionid, CancellationToken cancellationToken)
        {

            var ISPollExist = await _context.Polls.FindAsync(pollId, cancellationToken);

            if (ISPollExist == null)
            {

                return Result.Falire<QuestionResponse>(PollsErrors.PollNotFound);

            }

            var question = await _context.Questions.SingleOrDefaultAsync(x=>x.Pollid == pollId && x.Id == Questionid , cancellationToken);

            if (question == null)
            {
                return Result.Falire(QuestionsErrors.QuestionNotFound);

            }

            question.IsActive = !question.IsActive;

            await _context.SaveChangesAsync();

            await cacheService.RemoveAsync($"{_CachePrefix}-{pollId}", cancellationToken);


            return Result.Success();
        }
    }
}
