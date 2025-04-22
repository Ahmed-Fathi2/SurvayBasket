using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SurvayBasket.Abstractions.Consts.cs;
using SurvayBasket.Contracts.Common.cs;
using SurvayBasket.Contracts.Questions;
using SurvayBasket.Errors;

using SurvayBasket.UsreErrors;

namespace SurvayBasket.Controllers
{
    [Route("api/Polls/{pollId}/[controller]")]
    [ApiController]
    //[Authorize(Roles =DefaultRole.Admin)]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService  _questionService;
         

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

  

        [HttpGet]
        
        public async Task<IActionResult> GetAll([FromRoute] int pollId,[FromQuery] RequestFilters filters ,  CancellationToken cancellationToken)
        {

            var result = await _questionService.GetAllQuestionsAsync(pollId, filters, cancellationToken);

            return result.IsSuccess ? Ok(result.Value()) : result.ToProblem(statuscode: StatusCodes.Status404NotFound);
            
            
            
        }


        [HttpGet("{Questionid}")]
      
        public async Task<IActionResult> Get([FromRoute] int pollId,[FromRoute] int Questionid , CancellationToken cancellationToken  )
        {
            var result = await _questionService.GetAsync(pollId, Questionid, cancellationToken);

            return result.IsSuccess ? Ok(result.Value()) : result.ToProblem(statuscode: StatusCodes.Status404NotFound);

        }



        [HttpPost]
        
        public async Task<IActionResult> Add ([FromRoute] int pollId , [FromBody] QuestionRequest questionRequest , CancellationToken cancellationToken)
        {
            var result = await _questionService.AddAsync(pollId , questionRequest , cancellationToken);

            if (result.IsSuccess) 
            {
            return CreatedAtAction(nameof(Get), new { pollId, Questionid = result.Value()!.id }, result.Value());



            }

            return result.Error.Equals(QuestionsErrors.DublicatedQuestionInTheSamePoll)?
                           result.ToProblem(statuscode: StatusCodes.Status409Conflict):
                           result.ToProblem(statuscode: StatusCodes.Status404NotFound);

     

            }



        [HttpPut("{Questionid}")]

        public async Task<IActionResult> Update ([FromRoute] int pollId, [FromRoute] int Questionid, [FromBody] QuestionRequest questionRequest , CancellationToken cancellationToken)
        {
            var result = await _questionService.UpdateAsync(pollId , Questionid, questionRequest , cancellationToken);

            if (result.IsSuccess) 
            {
                return NoContent();

            }

            if (result.Error.Equals(PollsErrors.PollNotFound))
            {


                return result.ToProblem(statuscode:StatusCodes.Status404NotFound);
            }


            return result.Error.Equals(QuestionsErrors.DublicatedQuestionInTheSamePoll)?
                           result.ToProblem(statuscode: StatusCodes.Status409Conflict):
                           result.ToProblem(statuscode: StatusCodes.Status404NotFound);

     

            }



        [HttpPut("{Questionid}/Toggle")]

        public async Task<IActionResult> Toggle([FromRoute] int pollId, [FromRoute] int Questionid, CancellationToken cancellationToken)
        {
            var result = await _questionService.ToggleAsync(pollId, Questionid, cancellationToken);

            return (result.IsSuccess) ? NoContent() : result.ToProblem(statuscode: StatusCodes.Status404NotFound);

        }

    }

}

