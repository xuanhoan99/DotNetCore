using HCore.Application.Modules.Common.Responses;
using System.Net;
using System.Text.Json;

namespace HCore.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");
                await HandleExceptionAsync(context, ex, _env.IsDevelopment());
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex, bool isDev)
        {
            var statusCode = ex switch
            {
                ArgumentNullException or ArgumentException => HttpStatusCode.BadRequest,
                UnauthorizedAccessException => HttpStatusCode.Forbidden,
                KeyNotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError
            };

            var message = isDev ? ex.Message : "Đã xảy ra lỗi không mong muốn.";
            var details = isDev ? ex.StackTrace : null;

            var response = BaseResponse<string>.Fail(message);
            if (isDev && details != null)
                response.ErrorCode = details;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            var json = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(json);
        }
    }
}
