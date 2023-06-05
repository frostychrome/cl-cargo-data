namespace Cargo.Data.Core.Factories;

public class SensorFactory : ISensorFactory
{
    public BaseSensor CreateSensor(SensorType sensorType, Action<BaseSensor>? initializer = default)
    {
        BaseSensor sensor = sensorType switch
        {
            SensorType.Temperature => new TemperatureSensor(),
            SensorType.Humidity => new HumiditySensor(),
            _ => throw new ArgumentOutOfRangeException(nameof(sensorType), sensorType, $"Unsupported sensor type {sensorType}")
        };
        initializer?.Invoke(sensor);
        return sensor;
    }
}
