using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurvayBasket.Abstractions.Consts.cs;

namespace SurvayBasket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles =DefaultRole.Admin)]
    public class ResultsController(IVoteService voteService) : ControllerBase
    {
        private readonly IVoteService voteService = voteService;


        [HttpGet("Results")]
        public async Task<IActionResult> GetResults([FromRoute] int pollId, CancellationToken cancellationToken)
        {

            var result = await voteService.GetVoteResults(pollId, cancellationToken);

            return (result.IsSuccess) ? Ok(result.Value()) : result.ToProblem(statuscode: StatusCodes.Status404NotFound);

        }


        [HttpGet("Results/Day")]
        public async Task<IActionResult> GetResultsPerDay([FromRoute] int pollId, CancellationToken cancellationToken)
        {

            var result = await voteService.GetVoteResultsPerDay(pollId, cancellationToken);

            return (result.IsSuccess) ? Ok(result.Value()) : result.ToProblem(statuscode: StatusCodes.Status404NotFound);

        }


        [HttpGet("Results/Question")]
        public async Task<IActionResult> GetResultsPerQuestion([FromRoute] int pollId, CancellationToken cancellationToken)
        {

            var result = await voteService.GetVoteResultsPerQuestion(pollId, cancellationToken);

            return (result.IsSuccess) ? Ok(result.Value()) : result.ToProblem(statuscode: StatusCodes.Status404NotFound);

        }
    }
}
