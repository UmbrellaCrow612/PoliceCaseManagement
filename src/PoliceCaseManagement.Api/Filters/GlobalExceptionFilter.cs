using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using PoliceCaseManagement.Application.DTOs.Errors;
using PoliceCaseManagement.Api.Exceptions;

namespace PoliceCaseManagement.Api.Filters
{
    public class GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IHostEnvironment env) : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger = logger;
        private readonly IHostEnvironment _env = env;

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "An unhandled exception occurred.");

            var errorResponse = new ErrorResponseDto(
                message: "An error occurred while processing your request.",
                details: _env.IsDevelopment() ? context.Exception.StackTrace : null
            );

            if (context.Exception is ApiException apiException)
            {
                errorResponse.Message = context.Exception.Message;
                context.Result = new ObjectResult(errorResponse)
                {
                    StatusCode = apiException.StatusCode
                };
            }
            else
            {
                context.Result = new ObjectResult(errorResponse)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            context.ExceptionHandled = true;
        }
    }
}
