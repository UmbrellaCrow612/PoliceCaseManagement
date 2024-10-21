using Microsoft.AspNetCore.Mvc;

namespace PoliceCaseManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "Hello" };
        }
    }
}
