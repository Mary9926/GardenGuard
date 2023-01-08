using Azure.Data.Tables;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace GardenGuardFrontend.Server;

public class SensorService
{
    private readonly AzureStorageSettings _storageSettings;
    private TableClient _tableClient { get; }

    public SensorService(IOptions<AzureStorageSettings> storageSettings)
    {
        _storageSettings = storageSettings.Value;
        _tableClient = GetTableClient();
    }

    private TableClient GetTableClient()
    {
        TableServiceClient tableStorage = new(_storageSettings.ConnectionString);
        tableStorage.CreateTableIfNotExists(_storageSettings.SensorsTableName);
        return new TableClient(_storageSettings.ConnectionString, _storageSettings.SensorsTableName);
    }

    public async Task<SensorData?> GetSensorDataAsync(string deviceId)
    {
        var entityResponse = await _tableClient.GetEntityAsync<SensorDataEntity>(deviceId, deviceId);
        var responseContent = entityResponse?.GetRawResponse()?.Content;
        var entity = JsonSerializer.Deserialize<SensorDataEntity>(responseContent);
        return entity?.GetSensorsData();
    }
}
