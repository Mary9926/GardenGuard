using Microsoft.AspNetCore.Mvc;

namespace GardenGuardFrontend.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SensorController : ControllerBase
{
    private SensorService _sensorsService { get; set; }

    public SensorController(SensorService sensorService)
    {
        _sensorsService = sensorService;
    }

    [HttpGet("{deviceId}")]
    public async Task<IActionResult> GetCurrentSensorData(string deviceId)
    {
        var sensorsData = await _sensorsService.GetSensorDataAsync(deviceId);

        if (sensorsData is null)
            return NotFound();

        return Ok(sensorsData);
    }
}
