using RealEstateHub.Application.Common;
using RealEstateHub.Application.Exceptions;
using System.Net;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RealEstateHub.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment env)
        {
            _next = next;
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            var statusCode = HttpStatusCode.InternalServerError;
            string message = "An unexpected error occurred.";
            object? errors = null;

            switch (ex)
            {
                case NotFoundException nf:
                    statusCode = HttpStatusCode.NotFound;
                    message = nf.Message;
                    break;

                case BusinessException be:
                    statusCode = HttpStatusCode.BadRequest;
                    message = be.Message;
                    break;

                case ValidationException ve:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "Validation failed.";
                    errors = ve.Errors;
                    break;

                default:
                    if (_env.IsDevelopment())
                    {
                        message = ex.Message +
                                  (ex.InnerException != null ? " | Inner: " + ex.InnerException.Message : "");
                    }
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            var response = new ErrorResponse((int)statusCode, message, errors);

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }

}
