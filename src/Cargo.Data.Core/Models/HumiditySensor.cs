namespace Cargo.Data.Core.Models;

public class HumiditySensor : BaseSensor
{
    public override SensorType Type => SensorType.Humidity;
}