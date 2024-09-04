using Microsoft.AspNetCore.Mvc;

namespace YourNamespaceHere  
{
    [ApiController]
    [Route("[controller]")]  // Optional: Defines a route prefix for all actions in this controller
    public class HomeController : ControllerBase
    {
        [HttpGet("/")]
        public IActionResult Get() => Ok("API is running!");
    }
}
