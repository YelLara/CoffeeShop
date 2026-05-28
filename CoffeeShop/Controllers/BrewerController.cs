using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoffeeShop.Controllers
{
    [ApiController]
    public class BrewerController : ControllerBase
    {
        private static int _brewRequestCount;

        [HttpGet("/brewer-coffee")]
        public async Task<IActionResult> CoffeeBrew()
        {
            var currentCount = Interlocked.Increment(ref _brewRequestCount);
            var now = DateTime.Now;
            var result = new Coffee();
            var message = "Your piping hot coffee is ready!";

            if (now.Month == 4 && now.Day == 1)
            {
                return StatusCode(StatusCodes.Status418ImATeapot);
            }

            if (currentCount % 5 == 0)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            var data = await getTemperature();

            if (data is null)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            if (data.temperature_2m > 30)
            {
                message = "Your refreshing iced coffee is ready!";
            }

            result = new Coffee
            {
                message = message,
                prepared = DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:sszzz")
            };

            return Ok(result);
        }

        private async Task<WeatherResponse?> getTemperature()
        {
            string lat = "14.6091";
            string lon = "121.0223";

            var url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current=temperature_2m";

            using var client = new HttpClient();
            string json = await client.GetStringAsync(url);

            var currentElement = JsonDocument.Parse(json)
                .RootElement
                .GetProperty("current");

            var data = JsonSerializer.Deserialize<WeatherResponse>(currentElement);

            return data;
        }
    }

    class WeatherResponse
    {
        [JsonPropertyName("temperature_2m")]
        public double temperature_2m { get; set; }
    }
}
