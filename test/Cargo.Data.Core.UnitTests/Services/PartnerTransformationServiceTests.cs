using Cargo.Data.Core.Interfaces;
using Cargo.Data.Core.Models;
using Cargo.Data.Core.UnitTests.Fixtures;

namespace Cargo.Data.Core.Services.Tests;

public class PartnerTransformationServiceTests : IClassFixture<EntityFixture>
{
    private readonly EntityFixture fixture;
    private readonly Mock<ILogger<PartnerTransformationService>> logger;
    private readonly Mock<IEntityTransformationService<Device>> devicerTransformationService;

    public PartnerTransformationServiceTests(EntityFixture fixture)
    {
        this.logger = new Mock<ILogger<PartnerTransformationService>>();
        this.devicerTransformationService = new Mock<IEntityTransformationService<Device>>();
        this.fixture = fixture;
    }

    [Fact()]
    public void Merge_ShouldNotMutate_WhenNoDataOverlap()
    {
        devicerTransformationService.Setup(m => m
            .Merge(It.IsAny<IEnumerable<Device>>()))
            .Returns((IEnumerable<Device> devices) => devices);

        var service = CreateServiceInstance();
        var result = service.Merge(fixture.Partners);
        result.Should().BeEquivalentTo(fixture.Partners);
    }

    [Fact()]
    public void Merge_ShouldMerge_WhenMergeWithSelf()
    {
        var testData = new[] { fixture.Partners[0], fixture.Partners[0] };

        devicerTransformationService.Setup(m => m
            .Merge(It.IsAny<IEnumerable<Device>>()))
            .Returns(testData[0].Devices);

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
        var result = service.Merge(new Partner[] { });
        result.Should().BeEmpty();
    }

    private PartnerTransformationService CreateServiceInstance()
    {
        return new PartnerTransformationService(logger.Object, devicerTransformationService.Object);
    }
}