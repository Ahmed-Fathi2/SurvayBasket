using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurvayBasket.Contracts.AccountProfile.cs;
using SurvayBasket.services;

namespace SurvayBasket.Controllers
{
    [Route("Account")]
    [ApiController]
    [Authorize]
    public class AccountsController(IAccountService accountService) : ControllerBase
    {
        private readonly IAccountService accountService = accountService;


        [HttpGet("User-Info")]
        public async Task<ActionResult> UserInfo()
        {

            var userInfo = await accountService.GetUserProfileAsync();

            return Ok(userInfo.Value());


        }


        [HttpPut("Update-Info")]
        public async Task<ActionResult> UpdateUserInfo(UserUpdatedProfileRequest request)
        {

            var userInfo = await accountService.UpdateUserProfileAsync(request);

            return NoContent();


        }


        [HttpPut("change-Password")]
        public async Task<ActionResult> ChangeUserPassword(ChangePassWordRequest request)
        {

            var userInfo = await accountService.ChangeUserPassword(request);

            return userInfo.IsSuccess ? NoContent() : userInfo.ToProblem(statuscode: StatusCodes.Status400BadRequest);


        }


        [HttpPost("Forget-Password")]
        [AllowAnonymous]
        public async Task<ActionResult> ForgetUserPassword(ForgetPasswordRequest request)
        {

            var userInfo = await accountService.ForgetUserPassword(request);

            return Ok();


        }


        [HttpPost("Reset-Password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetUserPassword(ResetPasswordRequest Request)
        {

            var result = await accountService.ResetUserPassword(Request);

            if (result.IsSuccess)
                return Ok();

            return result.Error.Equals(UsersErrors.InvalidCode) ?
                        result.ToProblem(statuscode: StatusCodes.Status400BadRequest) 
                      : result.ToProblem(statuscode: StatusCodes.Status401Unauthorized);

           
        }

    }
}
