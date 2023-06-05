using Cargo.Data.Core.Enums;
using Cargo.Data.Core.Interfaces;
using Cargo.Data.Core.Models;
using Cargo.Data.Core.Models.Base;
using Cargo.Data.Core.UnitTests.Fixtures;

namespace Cargo.Data.Core.Services.Tests;

public class SensorTransformationServiceTests : IClassFixture<EntityFixture>
{
    private readonly EntityFixture fixture;
    private readonly Mock<ILogger<SensorTransformationService>> logger;
    private readonly Mock<ISensorFactory> sensorFactory;
    private readonly Mock<IEntityTransformationService<Measurement>> measurementTransformationService;

    public SensorTransformationServiceTests(EntityFixture fixture)
    {
        this.logger = new Mock<ILogger<SensorTransformationService>>();
        this.sensorFactory = new Mock<ISensorFactory>();
        this.measurementTransformationService = new Mock<IEntityTransformationService<Measurement>>();
        this.fixture = fixture;
    }

    [Fact()]
    public void Merge_ShouldNotMutate_WhenNoDataOverlap()
    {
        var setup = sensorFactory.SetupSequence(m => m
                .CreateSensor(It.IsAny<SensorType>(), It.IsAny<Action<BaseSensor>>()));
        foreach (var senspr in fixture.Sensors)
                setup.Returns(senspr);
        var service = CreateServiceInstance();
        var result = service.Merge(fixture.Sensors);
        result.Should().BeEquivalentTo(fixture.Sensors);
    }

    [Fact()]
    public void Merge_ShouldPass_WhenMergeWithSelf()
    {
        var testData = new[] { fixture.Sensors[0], fixture.Sensors[0] };
        sensorFactory.Setup(m => m
            .CreateSensor(It.IsAny<SensorType>(), It.IsAny<Action<BaseSensor>>()))
            .Returns(testData[0]);
        var service = CreateServiceInstance();
        var result = service.Merge(testData).ToList();
        result.Should().BeEquivalentTo(new[] { fixture.Sensors[0] });
    }

    [Fact()]
    public void Merge_ShouldThrow_WhenNull()
    {
        var service = CreateServiceInstance();
        FluentActions.Invoking(() => service.Merge(default).ToList())
            .Should().Throw<ArgumentNullException>();
    }

    [Fact()]
    public void Merge_ReturnEmpty_WhenInputIsEmpty()
    {
        var service = CreateServiceInstance();
        var result = service.Merge(new TemperatureSensor[] { });
        result.Should().BeEmpty();
    }

    private SensorTransformationService CreateServiceInstance()
    {
        return new SensorTransformationService(logger.Object, sensorFactory.Object, measurementTransformationService.Object);
    }
}
