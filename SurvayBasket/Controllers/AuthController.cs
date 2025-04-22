using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SurvayBasket.Authentication;

namespace SurvayBasket.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
    {
        private readonly IAuthService authService = authService;
        private readonly ILogger<AuthController> logger = logger;


        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginRequest loginRequest,CancellationToken cancellationToken)
        {
            logger.LogInformation("Logging with email: {email} and password: {password}", loginRequest.Email, loginRequest.Password);


            var authresult = await authService.GetTokenAsync(loginRequest.Email, loginRequest.Password , cancellationToken);

           
             if (authresult.IsSuccess)
                return Ok(authresult.Value());

            if (authresult.Error.Equals(UsersErrors.IvalidCredential))
                return authresult.ToProblem(statuscode: StatusCodes.Status400BadRequest);

            else
                return authresult.ToProblem(statuscode: StatusCodes.Status401Unauthorized);

        }


        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest Request, CancellationToken cancellationToken)
        {

            var authresult = await authService.RegisterAsync(Request, cancellationToken);

            if (authresult.IsSuccess)
                return Ok("Registration completed successfully , Please confirm your Email ");

            if (authresult.Error.Equals(UsersErrors.DublicatedEmail))
                return authresult.ToProblem(statuscode: StatusCodes.Status409Conflict);

            else
                return  authresult.ToProblem(statuscode: StatusCodes.Status400BadRequest);

        }


        [HttpPost("Confirm-Email")]
        public async Task<IActionResult> ConfirmEmailAsync(ConfirmationEmailRequest Request, CancellationToken cancellationToken)
        {

            var authresult = await authService.ConfirmEmailAsync(Request, cancellationToken);

            if (authresult.IsSuccess)
                return Ok("Confirmation completed successfully , You can Login now ");

            if (authresult.Error.Equals(UsersErrors.InvalidCode))
                return authresult.ToProblem(statuscode: StatusCodes.Status401Unauthorized);

            else
                return authresult.ToProblem(statuscode: StatusCodes.Status400BadRequest);

        }


        [HttpPost("Resend-Confirm-Email")]
        public async Task<IActionResult> ResendConfirmEmailAsync(ResendConfirmationEmail Request, CancellationToken cancellationToken)
        {

            var authresult = await authService.ResendConfirmEmailAsync(Request, cancellationToken);

            return (authresult.IsSuccess) ?  Ok("Email is Resent again") : authresult.ToProblem(statuscode: StatusCodes.Status400BadRequest);
        }


    }
}
