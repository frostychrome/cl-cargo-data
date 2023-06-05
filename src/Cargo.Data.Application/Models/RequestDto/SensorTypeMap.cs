using Cargo.Data.Core.Enums;

namespace Cargo.Data.Application.Models.RequestDto;
public static class SensorTypeMap
{
    public static Dictionary<string, SensorType> Foo1SensorTypes { get; } = new Dictionary<string, SensorType>()
    {
        ["Temperature"] = SensorType.Temperature,
        ["Humidty"] = SensorType.Humidity,
    };

    public static Dictionary<string, SensorType> Foo2SensorTypes { get; } = new Dictionary<string, SensorType>()
    {
        ["TEMP"] = SensorType.Temperature,
        ["HUM"] = SensorType.Humidity,
    };

    public static SensorType MapFoo1SensorType(string sensorType) =>
        Foo1SensorTypes.GetValueOrDefault(sensorType ?? string.Empty, SensorType.None);

    public static SensorType MapFoo2SensorType(string sensorType) =>
        Foo2SensorTypes.GetValueOrDefault(sensorType ?? string.Empty, SensorType.None);
}
