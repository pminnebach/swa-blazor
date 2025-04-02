using System;
using System.Net;
using System.Threading.Tasks;
using BlazorApp.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;

namespace Api
{
    public class TimeRegistrationFunction
    {
        private readonly ILogger _logger;

        public TimeRegistrationFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TimeRegistrationFunction>();
        }

        [Function("AddTimeRegistration")]
        public async Task<HttpResponseData> AddTimeRegistration(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed AddTimeRegistration request.");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var timeRegistration = JsonSerializer.Deserialize<TimeRegistration>(
                    requestBody, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (timeRegistration == null)
                {
                    var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                    await badResponse.WriteStringAsync("Invalid time registration data.");
                    return badResponse;
                }

                // Generate a new ID if not provided
                if (timeRegistration.Id == Guid.Empty)
                {
                    timeRegistration.Id = Guid.NewGuid();
                }

                // Here you would typically save the time registration to a database
                // For now, we'll just return the created time registration with its ID

                var response = req.CreateResponse(HttpStatusCode.Created);
                await response.WriteAsJsonAsync(timeRegistration);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing AddTimeRegistration request");
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("Error processing request: " + ex.Message);
                return errorResponse;
            }
        }
    }
} 