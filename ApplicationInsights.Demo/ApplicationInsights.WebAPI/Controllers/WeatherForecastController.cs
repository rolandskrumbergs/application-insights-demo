using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace ApplicationInsights.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public async Task<IActionResult> CallFunction()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://ai-demo-8asdf873.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.PostAsJsonAsync("api/demo", new { test = 1 });
            response.EnsureSuccessStatusCode();

            var connectionString = "DefaultEndpointsProtocol=https;AccountName=aidemowebapqueue;AccountKey=KWrbuVhwE1zL0z9JNXVGUT5xUhchiHysXSOrQp5lHZPQkLEaFgv0jrh+6xjLTAoRCDnWVd5Zw7m6+AStIF9qeA==;EndpointSuffix=core.windows.net";
            var queueName = "aidemo";
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            await queueClient.CreateIfNotExistsAsync();

            var message = $"{Guid.NewGuid()} dmeo";
            queueClient.SendMessage(message);

            return Ok();
        }
    }
}