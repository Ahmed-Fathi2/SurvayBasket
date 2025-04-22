using Microsoft.AspNetCore.Authorization;
using SurvayBasket.Abstractions.Consts.cs;
namespace SurvayBasket.Controllers

{
    [Route("api/[controller]")]
    [ApiController]


    public class PollsController(IPollService pollService) : ControllerBase
    {
        private IPollService PollService { get; } = pollService;

        [HttpGet("")]
        [HasPermission(Permissions.GetPolls)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await PollService.GetAllAsync(cancellationToken);
            return Ok(result.Value());
        }

        [HttpGet("current Available Polls ")]
        [Authorize(Roles =DefaultRole.Member)]
        public async Task<IActionResult> GetAvailable(CancellationToken cancellationToken)
        {
            var result = await PollService.GetAllAvailableAsync(cancellationToken);
            return Ok(result.Value());

        }


        [HttpGet("{id}")]
       
        public async Task<IActionResult> Get([FromRoute] int id,CancellationToken cancellationToken)
        {

            var result = await PollService.GetAsync(id, cancellationToken);

            return result.IsSuccess ? Ok(result.Value()):
                                     result.ToProblem(statuscode: StatusCodes.Status404NotFound);
        }


        [HttpPost("")]

 
        public async Task<IActionResult> AddPoll([FromBody] PollRequest request
                                                            , CancellationToken cancellationToken)
        {

            var newpoll = await PollService.AddAsync(request, cancellationToken);

            if (!newpoll.IsSuccess)
            {
                return newpoll.ToProblem(statuscode: StatusCodes.Status409Conflict);

            }

            return CreatedAtAction(nameof(Get), new { id = newpoll.Value()!.Id }, newpoll.Value());

        }

        [HttpPut("{id}")]
    
        public async Task<IActionResult> UpdatePoll([FromRoute] int id, [FromBody] PollRequest request
                                                                      , CancellationToken cancellationToken)
        {

            var IsUpdated = await PollService.UpdateAsync(id, request, cancellationToken);

            if (!IsUpdated.IsSuccess)
            {
                //return NotFound(IsUpdated.Error);

                if (IsUpdated.Error.Code == " Poll Not Found ")
                    return IsUpdated.ToProblem(statuscode: StatusCodes.Status404NotFound);

                else if(IsUpdated.Error.Code == " Dublication operation ")
                    return IsUpdated.ToProblem(statuscode: StatusCodes.Status409Conflict);
          
                ;
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
 
        public async Task<IActionResult> DeletePoll([FromRoute] int id, CancellationToken cancellationToken)
        {
            var ISDeleted = await PollService.DeleteAsync(id, cancellationToken);

            if (!ISDeleted.IsSuccess)
            {

                return ISDeleted.ToProblem(statuscode: StatusCodes.Status404NotFound);
            }

            return NoContent();
        }


        [HttpPut("{id}/togglepublish")]
 
        public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)

        {

            var IsToggled = await PollService.TogglePublishAsync(id, cancellationToken);
            if (!IsToggled.IsSuccess)
            {

                return IsToggled.ToProblem(statuscode: StatusCodes.Status404NotFound);

            }

            return NoContent();
        }




    }
}
