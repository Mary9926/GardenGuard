using Azure;
using Azure.Data.Tables;

namespace GardenGuardFrontend.Server;

public class SensorDataEntity : ITableEntity
{
    public float Temperature { get; set; }
    public float Humidity { get; set; }
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public SensorData GetSensorsData() => new SensorData()
    {
        Temperature = Temperature,
        Humidity = Humidity
    };
}
