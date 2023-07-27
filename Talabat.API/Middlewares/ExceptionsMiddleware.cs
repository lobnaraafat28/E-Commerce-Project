using System.Net;
using System.Text.Json;
using Talabat.API.Errors;

namespace Talabat.API.Middlewares
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionsMiddleware> logger;
        private readonly IHostEnvironment env;

        public ExceptionsMiddleware(RequestDelegate next,ILogger<ExceptionsMiddleware> logger,IHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode= (int) HttpStatusCode.InternalServerError;

                var response = env.IsDevelopment()?
                    new ApiExeptionResponse((int) HttpStatusCode.InternalServerError,ex.Message,ex.StackTrace.ToString())
                    : new ApiExeptionResponse((int)HttpStatusCode.InternalServerError);
                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var Json=JsonSerializer.Serialize(response,options);
                await context.Response.WriteAsync(Json);
            }
        }
    }
}
