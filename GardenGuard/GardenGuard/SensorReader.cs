using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.EventHubs;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Data.Tables;

namespace GardenGuard;

public class SensorReader
{        
    [FunctionName(nameof(SensorReader))]
    public async Task Run(
        [IoTHubTrigger("messages/events", Connection = "IoTHubConfig_ConnectionString")] EventData eventData,
        [Table("SensorsDataTable")] TableClient table,
        ILogger log)
    {
        var message = Encoding.UTF8.GetString(eventData.Body.Array);
        var sensorData = JsonSerializer.Deserialize<SensorData>(message);

        var deviceId = eventData.SystemProperties["iothub-connection-device-id"].ToString();

        var entity = await table.GetEntityAsync<SensorDataEntity>(deviceId, deviceId);
        await table.UpdateEntityAsync(new SensorDataEntity(deviceId, sensorData), entity.Value.ETag);

        log.LogInformation($"Temp: {sensorData.Temperature} / Humidity: {sensorData.Humidity}");
    }
}
