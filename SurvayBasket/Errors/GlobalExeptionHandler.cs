using Microsoft.AspNetCore.Diagnostics;

namespace SurvayBasket.Errors
{
    public class GlobalExeptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExeptionHandler> _logger;

        public GlobalExeptionHandler(ILogger<GlobalExeptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {

            _logger.LogError(exception,"sometimes went wrong : {Message} " , exception.Message);

            var problemdetails = new ProblemDetails()
            {
             Status= StatusCodes.Status500InternalServerError,
             Title= "Internal Server Error ",
             Type= "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"

            };
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problemdetails);

            return true;


        }
    }
}
