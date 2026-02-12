using System.Net;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;
        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware>logger, 
            RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();
                // Log the exception with the unique error ID
                logger.LogError(ex, $"{errorId}:{ex.Message}");
                //return a custom error response to the client
               httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
               httpContext.Response.ContentType = "application/json";
                var errorResponse = new
                {
                    ErrorId = errorId,
                    Message = "An unexpected error occurred. Please contact support with the Error ID."
                };
                await httpContext.Response.WriteAsJsonAsync(errorResponse);
            }
        }

    }
}
