namespace Cargo.Data.Core.Interfaces;

public interface ISensorFactory
{
    BaseSensor CreateSensor(SensorType sensorType, Action<BaseSensor>? initializer = default);
}
