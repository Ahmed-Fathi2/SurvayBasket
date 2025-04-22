namespace SurvayBasket.services

{
    public class PollService : IPollService
    {
        private readonly AppDbContext _context;
        public PollService(AppDbContext context)
        {
            _context=context;
        }


        public async Task<Result<IEnumerable<PollResponse>>> GetAllAsync( CancellationToken cancellationToken)
        {
            var polls = await _context.Polls.AsNoTracking().ToListAsync(cancellationToken);
            
            var PollResponse = polls.Adapt<IEnumerable<PollResponse>>();

           return Result.Success<IEnumerable<PollResponse>>(PollResponse);
        }

        public async Task<Result<PollResponse>> GetAsync(int id, CancellationToken cancellationToken)
        {
            var poll = await _context.Polls.FindAsync(id, cancellationToken);
            if (poll == null)
            {
                return Result.Falire<PollResponse>(PollsErrors.PollNotFound);
            }

            return Result.Success<PollResponse>(poll.Adapt<PollResponse>());
        }

        public async Task<Result<PollResponse>> AddAsync(PollRequest Request, CancellationToken cancellationToken = default)
        {
           
            var IsTitelExist = await _context.Polls.AnyAsync(x=>x.Title == Request.Title);

            if (IsTitelExist)  
                return Result.Falire<PollResponse>(PollsErrors.PollDuplications);

            var poll = Request.Adapt<Poll>();
            await _context.AddAsync(poll, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var pollresponse = poll.Adapt<PollResponse>();
            return Result.Success(pollresponse);
        }


        public async Task<Result> UpdateAsync(int id, PollRequest request, CancellationToken cancellationToken)
        {


            var poll = await _context.Polls.FindAsync(id, cancellationToken);

            if (poll is null)
                return Result.Falire(PollsErrors.PollNotFound);

            var IsTitleExist = await _context.Polls.AnyAsync(x => x.Title == request.Title && x.Id != id);

            if (IsTitleExist)
                return Result.Falire(PollsErrors.PollDuplications);

             request.Adapt(poll);

            ////poll.Title = request.Title;
            ////poll.Summary = request.Summary;
            ////poll.StartsAt = request.StartsAt;
            ////poll.EndsAt = request.EndsAt;



            //await _context.Polls.Where(x => x.Id == id)
            //            .ExecuteUpdateAsync(x => x.SetProperty(p => p.Title, request.Title)
            //                                      .SetProperty(p => p.Summary, request.Summary)
            //                                      .SetProperty(p => p.StartsAt, request.StartsAt)
            //                                      .SetProperty(p => p.EndsAt, request.EndsAt));

            await _context.SaveChangesAsync(cancellationToken);


            return Result.Success();

        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var poll = await _context.Polls.FindAsync(id, cancellationToken);


            if (poll is null)
                return Result.Falire(PollsErrors.PollNotFound);

            _context.Remove(poll);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> TogglePublishAsync(int id, CancellationToken cancellationToken = default)
        {
            var poll = await _context.Polls.FindAsync(id, cancellationToken);

            if (poll is null)
                return Result.Falire(PollsErrors.PollNotFound);

            poll.IsPublished = !poll.IsPublished;
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success(); 
        }

        public async Task<Result<IEnumerable<PollResponse>>> GetAllAvailableAsync(CancellationToken cancellationToken)
        {
            var AllAvailablePolls = await _context.Polls
                                      .Where(x=>x.IsPublished && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) && x.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow))
                                      .ProjectToType<PollResponse>()  
                                      .AsNoTracking()
                                      .ToListAsync();

            return Result.Success<IEnumerable<PollResponse>>(AllAvailablePolls);
        }
    }
}
