using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurvayBasket.Contracts.User.cs;
using SurvayBasket.services;

namespace SurvayBasket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService userService = userService;

        [HttpGet("")]
        public async Task<ActionResult> GetAll()
        {
            var result = await userService.GetAllAsync();
            return Ok(result.Value());

        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAll ([FromRoute] string id , CancellationToken cancellationToken)
        {
            var result = await userService.GetAsync(id,cancellationToken);
            return Ok(result.Value());

        }

        [HttpPost("")]
        public async Task<ActionResult> Add([FromBody] UserRequest request, CancellationToken cancellationToken)
        {
            var result = await userService.AddAsync(request, cancellationToken);

            if(result.IsSuccess) 
            return Ok(result.Value());

            return (result.Error.Equals(UsersErrors.DublicatedEmail))
                  ? result.ToProblem(statuscode: StatusCodes.Status409Conflict)
                  : result.ToProblem(statuscode: StatusCodes.Status400BadRequest);
                
                   

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromRoute] string id, [FromBody] UserUpdate request, CancellationToken cancellationToken)
        {
            var result = await userService.UpdateAsync(id, request, cancellationToken);

            if (result.IsSuccess)
                return NoContent();

            if (result.Error.Equals(UsersErrors.UserNotFound))
                return result.ToProblem(statuscode: StatusCodes.Status404NotFound);

            return (result.Error.Equals(UsersErrors.DublicatedEmail))
                  ? result.ToProblem(statuscode: StatusCodes.Status409Conflict)
                  : result.ToProblem(statuscode: StatusCodes.Status400BadRequest);



        }


        [HttpPut("{id}/Toggle-Status")]
        public async Task<ActionResult> Toggle([FromRoute] string id , CancellationToken cancellationToken)
        {
            var result = await userService.ToggleAsync(id,  cancellationToken);

            return (result.IsSuccess)
                    ? NoContent()
                    : result.ToProblem(statuscode: StatusCodes.Status404NotFound);

        }


        [HttpPut("{id}/UnLock")]
        public async Task<ActionResult> UnLock([FromRoute] string id, CancellationToken cancellationToken)
        {
            var result = await userService.UnLockAsync(id, cancellationToken);

            return (result.IsSuccess)
                    ? NoContent()
                    : result.ToProblem(statuscode: StatusCodes.Status404NotFound);

        }
    }
}
