using System.Net;
using System.Text.Json;
using Talabat.API.Errors;

namespace Talabat.API.Middlewares
{
    public class ExceptionMiddleware
    {

        private readonly RequestDelegate Next;
        private readonly ILogger<ExceptionMiddleware> Logger;
        private readonly IHostEnvironment Env;
        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger , IHostEnvironment env)
        {
            this.Next = next;
            this.Logger = logger;
            this.Env = env;
        }
        public  async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await Next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = Env.IsDevelopment() ? new ApiExceptionError((int)HttpStatusCode.InternalServerError , ex.Message,ex.StackTrace.ToString())
                    : new ApiExceptionError((int)HttpStatusCode.InternalServerError);
                var jsonoptions =new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var jsonresponse = JsonSerializer.Serialize(response , jsonoptions);
                await httpContext.Response.WriteAsync(jsonresponse);
               
            }
           
        }
    }
}
