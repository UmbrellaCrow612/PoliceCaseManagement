using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Challenge.Core.Responses
{
    internal class _401RedirectResult(string redirectUrl, string message) : IActionResult
    {
        public string RedirectUrl { get; set; } = redirectUrl;
        public string Message { get; set; } = message;

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = new
            {
                Message,
                RedirectUrl
            };

            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.HttpContext.Response.ContentType = "application/json";

            // Serialize the response to JSON and write to the body
            var json = System.Text.Json.JsonSerializer.Serialize(response);
            await context.HttpContext.Response.WriteAsync(json);
        }
    }
}
