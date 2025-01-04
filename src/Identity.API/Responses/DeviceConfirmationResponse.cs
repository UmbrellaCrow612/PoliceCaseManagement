using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Responses
{
    public static class DeviceConfirmationResponse
    {
        public static IActionResult GetResponse()
        {
            return new ObjectResult(new
            {
                redirectUrl = "authentication/device-challenge",
                message = "Device needs confirmation"
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }

        public static class Untrusted
        {
            public static IActionResult GetResponse()
            {
                return new ObjectResult(new
                {
                    redirectUrl = "authentication/device-challenge?trusted=false",
                    message = "Device needs confirmation"
                })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
        }
    }
}
