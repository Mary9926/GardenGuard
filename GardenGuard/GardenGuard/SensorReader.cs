using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.EventHubs;
using System.Text;
using Microsoft.Extensions.Logging;

namespace GardenGuard
{
    public class SensorReader
    {        
        [FunctionName(nameof(SensorReader))]
        public void Run(
            [IoTHubTrigger("messages/events", Connection = "IoTHubConfig_ConnectionString")] EventData message,
            ILogger log)
        {
            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");
        }
    }
}