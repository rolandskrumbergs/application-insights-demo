using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace ApplicationInsights.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://ai-demo-web-api-9g30g34.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.PostAsJsonAsync("WeatherForecast", new { test = 1 });
            response.EnsureSuccessStatusCode();

            var connectionString = "DefaultEndpointsProtocol=https;AccountName=aiwebapitable;AccountKey=B2Jvf4F17auicRBaxyRjvWJMhwb0TBo2CyGnyB5yH7+FuaUMoVBfKMZUU8vrdMAViy7InQik+Xgb+AStTTVFIw==;EndpointSuffix=core.windows.net";
            var serviceClient = new TableServiceClient(connectionString);

            string tableName = "demo";
            serviceClient.CreateTableIfNotExists(tableName);

            var tableClient = new TableClient(connectionString, tableName);

            var entity = new TableEntity(Guid.NewGuid().ToString(), Guid.NewGuid().ToString())
            {
                { "Product", Guid.NewGuid().ToString() },
                { "Price", 5.00 },
                { "Quantity", 21 }
            };

            tableClient.AddEntity(entity);

            return RedirectToPage("Index");
        }
    }
}