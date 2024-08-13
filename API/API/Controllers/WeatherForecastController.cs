using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
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
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            // nesse exemplo estou usando o profile instalado localmente
            var logClient = new AmazonCloudWatchLogsClient();
            var logGroupName = "/aws/weather-forecast-app";
            var logStreamName = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");

            var existing = await logClient
                    .DescribeLogGroupsAsync(new DescribeLogGroupsRequest()
                    {
                        LogGroupNamePrefix = logGroupName
                    });

            var logGroupExists = existing.LogGroups.Any(l => l.LogGroupName == logGroupName);

            if (!logGroupExists)
            {
                await logClient.CreateLogGroupAsync(new CreateLogGroupRequest(logGroupName));
            }

            await logClient.CreateLogStreamAsync(new CreateLogStreamRequest(logGroupName, logStreamName));
            await logClient.PutLogEventsAsync(new PutLogEventsRequest()
            {
                LogGroupName = logGroupName,
                LogStreamName = logStreamName,
                LogEvents = new List<InputLogEvent>()
                {
                    new InputLogEvent()
                    {
                        Message = "From get",
                        Timestamp = DateTime.UtcNow
                    }
                }
            });

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
