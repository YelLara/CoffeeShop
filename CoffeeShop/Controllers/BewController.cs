using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading;

namespace CoffeeShop.Controllers
{
    [ApiController]
    public class BrewController : ControllerBase
    {
        private static int _brewRequestCount;

        [HttpGet("/brew-coffee")]
        public IActionResult Coffee()
        {
            var currentCount = Interlocked.Increment(ref _brewRequestCount);
            var now = DateTime.Now;

            if (now.Month == 4 && now.Day == 1)
            {
                return StatusCode(StatusCodes.Status418ImATeapot);
            }

            if (currentCount % 5 == 0)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            var result = new Coffee
            {
                message = "Your piping hot coffee is ready",
                prepared = DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:sszzz")
            };

            return Ok(result);
        }
    }
}
