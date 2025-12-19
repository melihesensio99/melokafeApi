using KafeApi.Application.Dtos.ResponseDtos;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace kafeApi.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new ResponseDto<object>
            {
                Success = false,
                Message = "Sunucu Hatası: " + exception.Message,
                ErrorCode = ErrorCodes.EXCEPTION
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}