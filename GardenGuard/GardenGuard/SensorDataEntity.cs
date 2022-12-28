using Azure.Data.Tables;
using Azure;
using System;

namespace GardenGuard;

public class SensorDataEntity : ITableEntity
{
    public float Temperature { get; private set; }
    public float Humidity { get; private set; }
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public SensorDataEntity(string deviceId, SensorData sensorData)
    {
        PartitionKey = deviceId;
        RowKey = deviceId;
        Temperature = sensorData.Temperature;
        Humidity = sensorData.Humidity;
    }

    public SensorDataEntity()
    {
    }
}
