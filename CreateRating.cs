
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System;

namespace Company.Function
{
    public static class CreateRating
    {
        private static readonly HttpClient client = new HttpClient();

        [FunctionName("CreateRating")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req,
            [CosmosDB(
                databaseName: "openhack",
                collectionName: "ratings",
                ConnectionStringSetting = "CosmosDBConnection")] out Rating rating,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            rating = JsonConvert.DeserializeObject<Rating>(requestBody);

            var validProduct = ValidProduct(rating.productId);
            if (!validProduct)
            {
                return new BadRequestObjectResult("Bad product");
            }

            var validUser = ValidUser(rating.userId);
            if (!validUser)
            {
                return new BadRequestObjectResult("Bad user");
            }


            rating.id = Guid.NewGuid().ToString();
            rating.timestamp = DateTime.UtcNow.ToString();

            return (ActionResult)new OkObjectResult($"Request successful");
        }

        private static bool ValidUser(string userId)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var result = client.GetAsync($"{Settings.GetUserUrl}?userId={userId}").Result;
            return result.StatusCode == HttpStatusCode.OK;
        }

        private static bool ValidProduct(string productId)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var result = client.GetAsync($"{Settings.GetProductUrl}?productId={productId}").Result;
            return result.StatusCode == HttpStatusCode.OK;
        }
    }
}
