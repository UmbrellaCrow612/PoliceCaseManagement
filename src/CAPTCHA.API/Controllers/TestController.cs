using CAPTCHA.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace CAPTCHA.API.Controllers
{
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public ActionResult Test()
        {
            var test = CAPTCHAMathService.CreateQuestion();

            return Ok();
        }
    }
}
