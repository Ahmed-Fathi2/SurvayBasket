//using SurvayBasket.Contracts.Requests;
//using SurvayBasket.Contracts.Responses;

//namespace SurvayBasket.Mapping
//{
//    public static class ContractMapping
//    {
//        public static Poll MapToPoll(this PollRequest request)
//        {

//            return new Poll()
//            {

//                Title = request.Title,
//                Description = request.Description,


//            };

//        }


//        public static PollResponse MapToResponse(this Poll poll)
//        {

//            return new PollResponse()
//            {
//                Id = poll.Id,
//                Title = poll.Title,
//                Description = poll.Description,


//            };

//        }

//        public static IEnumerable<PollResponse> MapToResponse(this IEnumerable<Poll> poll)
//        {

//            return poll.Select(MapToResponse);

//        }
//    }
//}
