using System.Diagnostics;
using System.Net.Http;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Serilog;
using Application.Common.Exceptions;
using Configurations.Extentions;

namespace Configurations.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly Serilog.ILogger _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, Serilog.ILogger logger)
        {
            this.next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (AppException exception)
            {
                var response = ApiResponse<object>.Failed(exception.FriendlyMessage, exception.HttpStatusCode);
                await HandleExceptionAsync(context, response, exception, _logger, exception.HttpStatusCode);
            }
            catch (Exception exception)
            {
                var response = ApiResponse<object>.Failed(
                    $"Something went wrong. To help us resolve the issue please keep the 'Reference Id' handy. Reference Id: {Activity.Current?.Id}",
                    HttpStatusCode.InternalServerError);
                await HandleExceptionAsync(context, response, exception, _logger, HttpStatusCode.InternalServerError);
                Log.Error("error.");
                Log.Error($"Activity Id: {Activity.Current?.Id}");
                Log.Error(exception.UnwrapExceptionMessages());
                Log.Error(exception.InnerException?.UnwrapExceptionMessages() ?? "");
            }
        }

        private static Task HandleExceptionAsync(
            HttpContext context, ApiResponse<object> response, Exception exception, Serilog.ILogger logger, HttpStatusCode httpStatusCode)
        {


            Log.Error("****************************** API ERROR ******************************");
            Log.Error($"Activity Id: {Activity.Current?.Id}");
            Log.Error(exception.UnwrapExceptionMessages());
            var result =
                JsonSerializer.Serialize(
                    response,
                    MatchStyleOfExistingMvcProblemDetails());

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ((int)httpStatusCode);

            return context.Response.WriteAsync(result);

            static JsonSerializerOptions MatchStyleOfExistingMvcProblemDetails()
            {
                return new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.Never
                };
            }
        }
    }
}
