using Cargo.Data.Core.Models;
using Cargo.Data.Core.UnitTests.Fixtures;

namespace Cargo.Data.Core.Services.Tests;

public class MeasurementTransformationServiceTests : IClassFixture<EntityFixture>
{
    private readonly EntityFixture fixture;
    private readonly Mock<ILogger<MeasurementTransformationService>> logger;

    public MeasurementTransformationServiceTests(EntityFixture fixture)
    {
        this.logger = new Mock<ILogger<MeasurementTransformationService>>();
        this.fixture = fixture;
    }

    [Fact()]
    public void Merge_ShouldNotMutate_WhenNoDataOverlap()
    {
        var service = CreateServiceInstance();
        var result = service.Merge(fixture.Measurements);
        result.Should().BeEquivalentTo(fixture.Measurements);
    }

    [Fact()]
    public void Merge_ShouldPass_WhenMergeWithSelf()
    {
        var service = CreateServiceInstance();
        var result = service.Merge(new[] { fixture.Measurements[0], fixture.Measurements[0] });
        result.Should().BeEquivalentTo(new[] { fixture.Measurements[0] });
    }

    [Fact()]
    public void Merge_Throw_WhenMergeConflict()
    {
        var service = CreateServiceInstance();
        var measurements = new[]
        {
            new Measurement() { Timestamp = new DateTime(2020, 1, 1, 0, 0, 0), Value = 1 },
            new Measurement() { Timestamp = new DateTime(2020, 1, 1, 0, 0, 0), Value = 2 },
        };
        FluentActions.Invoking(() => service.Merge(measurements).ToList())
            .Should().Throw<ApplicationException>();
    }

    [Fact()]
    public void Merge_ShouldThrow_WhenNull()
    {
        var service = CreateServiceInstance();
        FluentActions.Invoking(() => service.Merge(null).ToList())
            .Should().Throw<ArgumentNullException>();
    }

    [Fact()]
    public void Merge_ReturnEmpty_WhenInputIsEmpty()
    {
        var service = CreateServiceInstance();
        var result = service.Merge(new Measurement[] { });
        result.Should().BeEmpty();
    }

    private MeasurementTransformationService CreateServiceInstance()
    {
        return new MeasurementTransformationService(logger.Object);
    }
}
