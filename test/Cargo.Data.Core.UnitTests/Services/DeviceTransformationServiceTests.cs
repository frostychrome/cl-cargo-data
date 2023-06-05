using Cargo.Data.Core.Interfaces;
using Cargo.Data.Core.Models;
using Cargo.Data.Core.Models.Base;
using Cargo.Data.Core.UnitTests.Fixtures;

namespace Cargo.Data.Core.Services.Tests;

public class DeviceTransformationServiceTests : IClassFixture<EntityFixture>
{
    private readonly EntityFixture fixture;
    private readonly Mock<ILogger<DeviceTransformationService>> logger;
    private readonly Mock<IEntityTransformationService<BaseSensor>> sensorTransformationService;

    public DeviceTransformationServiceTests(EntityFixture fixture)
    {
        this.logger = new Mock<ILogger<DeviceTransformationService>>();
        this.sensorTransformationService = new Mock<IEntityTransformationService<BaseSensor>>();
        this.fixture = fixture;
    }

    [Fact()]
    public void Merge_ShouldNotMutate_WhenNoDataOverlap()
    {
        sensorTransformationService.Setup(m => m
            .Merge(It.IsAny<IEnumerable<BaseSensor>>()))
            .Returns((IEnumerable<BaseSensor> sensors) => sensors);

        var service = CreateServiceInstance();
        var result = service.Merge(fixture.Devices);
        result.Should().BeEquivalentTo(fixture.Devices);
    }

    [Fact()]
    public void Merge_ShouldMerge_WhenMergeWithSelf()
    {
        var testData = new[] { fixture.Devices[0], fixture.Devices[0] };

        sensorTransformationService.Setup(m => m
            .Merge(It.IsAny<IEnumerable<BaseSensor>>()))
            .Returns(testData[0].Sensors);

        var service = CreateServiceInstance();
        var result = service.Merge(testData).ToList();
        result.Should().BeEquivalentTo(testData.Take(1));
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
        var result = service.Merge(new Device[] { });
        result.Should().BeEmpty();
    }

    private DeviceTransformationService CreateServiceInstance()
    {
        return new DeviceTransformationService(logger.Object, sensorTransformationService.Object);
    }
}
