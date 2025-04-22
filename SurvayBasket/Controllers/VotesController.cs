using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurvayBasket.Abstractions.Consts.cs;
using SurvayBasket.Contracts.Answers;
using SurvayBasket.Errors;
using SurvayBasket.UsreErrors;

namespace SurvayBasket.Controllers
{
    [Route("api/Poll/{pollId}/[controller]")]
    [ApiController]
   
    public class VotesController(IQuestionService questionService, IVoteService voteService) : ControllerBase
    {
        private readonly IQuestionService _questionService = questionService;
        private readonly IVoteService voteService = voteService;

        [HttpGet("")]
        public async Task<IActionResult> Start([FromRoute] int pollId, CancellationToken cancellationToken)
        {

            var result = await _questionService.GetAllAvailable(pollId, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Value());

            return result.Error.Equals(UsersErrors.ISVotedBefore) ?
                           result.ToProblem(statuscode: StatusCodes.Status409Conflict) :
                           result.ToProblem(statuscode: StatusCodes.Status404NotFound);


        }


        [HttpPost("")]
        public async Task<IActionResult> Add([FromRoute] int pollId, [FromBody] VoteRequest voteRequest, CancellationToken cancellationToken)
        {

            var result = await voteService.AddVote (pollId, voteRequest, cancellationToken);

            if (result.IsSuccess)
                return Created();

            if (result.Error.Equals(QuestionsErrors.QuestionNotFound))
            {
                return result.ToProblem(statuscode: StatusCodes.Status400BadRequest);

            }
            return result.Error.Equals(UsersErrors.ISVotedBefore) ?
                           result.ToProblem(statuscode: StatusCodes.Status409Conflict) :
                           result.ToProblem(statuscode: StatusCodes.Status404NotFound);


        }



  
    }

}
