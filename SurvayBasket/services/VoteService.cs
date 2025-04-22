namespace SurvayBasket.services

{
    public class VoteService : IVoteService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public VoteService(IHttpContextAccessor httpContextAccessor , AppDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<Result> AddVote(int pollid, VoteRequest voteRequest, CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var NoUserAccesToThisPoll = await _context.Votes.AnyAsync(x => x.PollId == pollid && x.UserId == userId); // check if user vote the poll before or not

            if (NoUserAccesToThisPoll)
            {

                return Result.Falire<IEnumerable<QuestionResponse>>(UsersErrors.ISVotedBefore);

            }

            // check if poll is availale or not 
            var IsAvailablePoll = await _context.Polls
                                    .AnyAsync(x => x.Id == pollid && x.IsPublished && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) && x.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken);




            if (!IsAvailablePoll)
            {

                return Result.Falire<IEnumerable<QuestionResponse>>(PollsErrors.PollNotFound);

            }


            var questionsIds= await _context.Questions.Where(x=>x.Pollid ==  pollid && x.IsActive).Select(x=>x.Id).ToListAsync();

            var ids = voteRequest.answers.Select(x => x.QuestionId);

            if (!(ids.SequenceEqual(questionsIds)))
                {

                return Result.Falire(QuestionsErrors.IvalidQuestion);

                };



            var IncomingAnswers = voteRequest.answers;

            foreach(var question in questionsIds)
            {

                var OrderedQuestAnswer = IncomingAnswers.Where(x => x.QuestionId == question).Select(x => x.AnswerId).ToList();
                var QuestAnswer = await _context.Answers.Where(x=>x.QuestionId == question).Select(x=>x.Id).ToListAsync();
                if (!OrderedQuestAnswer.Any(answer => QuestAnswer.Contains(answer)))
                {

                    return Result.Falire(AnswerErrors.IvalidAnswer);
                }


            }



            var Votes = new Vote
            {
                PollId = pollid,
                UserId = userId,
                VoteAnswers = voteRequest.answers.Adapt<IEnumerable<VoteAnswer>>().ToList()


            };
        


            await _context.AddAsync(Votes , cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();





        }

       
        public async Task<Result<PollVotesResponse>> GetVoteResults(int pollid, CancellationToken cancellationToken = default)
        {


            var VoteResults = await _context.Polls.Where(x => x.Id == pollid)
                .Select(x => new PollVotesResponse(

                    x.Title,
                    x.Votes.Select(x => new VoteResponse($"{x.User.FirstName}  {x.User.LastName}", x.SubmittedOn,
                                                          x.VoteAnswers.Select(x => new AnswerOfQuestions(x.Question.Content, x.Answer.Content))))


                    )).SingleOrDefaultAsync(cancellationToken);


            if (VoteResults == null)
            {
                return Result.Falire<PollVotesResponse>(PollsErrors.PollNotFound);

            }

            return Result.Success(VoteResults);





        }

        public async Task<Result<IEnumerable<VoteResultsResponsePerDay>>> GetVoteResultsPerDay(int pollid, CancellationToken cancellationToken = default)
        {
            var VoteResultsResponsePerDay = await _context.Votes.Where(x => x.PollId == pollid)
                .GroupBy(x => new { Date = DateOnly.FromDateTime(x.SubmittedOn)})
                 .Select(a => new VoteResultsResponsePerDay(

                     a.Key.Date,
                     a.Count()

                     )).ToListAsync(cancellationToken);


            if (VoteResultsResponsePerDay == null)
            {
                return Result.Falire<IEnumerable<VoteResultsResponsePerDay>>(PollsErrors.PollNotFound);

            }

            return Result.Success<IEnumerable<VoteResultsResponsePerDay>>(VoteResultsResponsePerDay);



        }

        public async Task<Result<IEnumerable<VotePerQuestion>>> GetVoteResultsPerQuestion(int pollid, CancellationToken cancellationToken = default)
        {

            var VotePerQuestion = await _context.Questions.Where(x => x.Pollid == pollid).
                Select(x => new VotePerQuestion(

                    x.Content,
                    x.VoteAnswers.GroupBy(x => new { AnswersContent = x.Answer.Content , AnswerId =x.Answer.Id  }).Select(a => new VotePerAnswer(

                        a.Key.AnswersContent,
                        a.Count()
                        
                        )
                    
                    ))).ToListAsync(cancellationToken);


            if (VotePerQuestion == null)
            {
                return Result.Falire < IEnumerable < VotePerQuestion >>(PollsErrors.PollNotFound);

            }

            return Result.Success <IEnumerable<VotePerQuestion>> (VotePerQuestion);


        }
    }
}
