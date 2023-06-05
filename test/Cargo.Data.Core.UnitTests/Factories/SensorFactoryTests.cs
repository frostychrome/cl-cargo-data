using Cargo.Data.Core.Enums;
using Cargo.Data.Core.Factories;
using Cargo.Data.Core.Models;

namespace Cargo.Data.Core.UnitTests.Factories;

public class SensorFactoryTests
{
    private readonly SensorFactory sensorFactory;

    public SensorFactoryTests()
    {
        sensorFactory = new SensorFactory();
    }

    [Theory]
    [InlineData(SensorType.Temperature, typeof(TemperatureSensor))]
    [InlineData(SensorType.Humidity, typeof(HumiditySensor))]
    public void CreateSensor_ShouldCreateExpectedDerivedType(SensorType sensorType, Type expectedType)
    {
        var sensor = sensorFactory.CreateSensor(sensorType);
        sensor.GetType().Should().Be(expectedType);
    }

    [Fact]
    public void CreateSensor_ShouldThrowWhenUnsupportedSensorType()
    {
        FluentActions.Invoking(() => sensorFactory.CreateSensor(SensorType.None))
            .Should().Throw<ArgumentOutOfRangeException>();
    }
}