using System.Text.Json.Serialization;

namespace GardenGuard;

public class SensorData
{
    [JsonPropertyName("temperature")]
    public float Temperature { get; set; }

    [JsonPropertyName("humidity")]
    public float Humidity { get; set; }
}
