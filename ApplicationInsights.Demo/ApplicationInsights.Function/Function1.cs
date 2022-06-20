using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using System.Text;

namespace ApplicationInsights.Function
{
    public static class Function1
    {
        [FunctionName("demo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var connectionString = "DefaultEndpointsProtocol=https;AccountName=aiwebapitable;AccountKey=B2Jvf4F17auicRBaxyRjvWJMhwb0TBo2CyGnyB5yH7+FuaUMoVBfKMZUU8vrdMAViy7InQik+Xgb+AStTTVFIw==;EndpointSuffix=core.windows.net";
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            string containerName = "quickstartblobs";

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            BlobClient blob = containerClient.GetBlobClient(Guid.NewGuid().ToString());

            var content = Encoding.UTF8.GetBytes($"{Guid.NewGuid()}");
                        using (var ms = new MemoryStream(content))
                            blob.Upload(ms);

            return new OkObjectResult("OK");
        }
    }
}
