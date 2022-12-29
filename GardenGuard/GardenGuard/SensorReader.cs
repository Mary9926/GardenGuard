using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.EventHubs;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GardenGuard;

public class SensorReader
{        
    [FunctionName(nameof(RecieveSensorData))]
    public async Task RecieveSensorData(
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

    [FunctionName(nameof(ReadCurrentSensorData))]
    public async Task<IActionResult> ReadCurrentSensorData(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "data")] HttpRequest req,
        [Table("SensorsDataTable")] TableClient table)
    {
        string deviceId = req.Query["deviceId"];

        if (string.IsNullOrEmpty(deviceId))
        {
            return new NotFoundResult();
        }

        var entityResponse = await table.GetEntityAsync<SensorDataEntity>(deviceId, deviceId);
        var responseContent = entityResponse.GetRawResponse().Content;
        var entity = JsonSerializer.Deserialize<SensorDataEntity>(responseContent);
        var sensorData = JsonSerializer.Serialize(entity.GetSensorsData());

        return new OkObjectResult(sensorData);
    }
}
