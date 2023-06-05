using Cargo.Data.Application.Interfaces.DataHandlers;
using Cargo.Data.Application.Models.ResponseDto;
using Cargo.Data.Application.UnitTests.Fixtures;
using Cargo.Data.Core.Interfaces;
using Cargo.Data.Core.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;

namespace Cargo.Data.Application.Services.Tests;

public class DataTransformationServiceTests : IClassFixture<EntityDataFixture>
{
    private readonly EntityDataFixture dataFixture;
    private readonly Mock<ILogger<DataTransformationService>> logger;
    private readonly Mock<IDataLoaderService> dataLoaderService;
    private readonly Mock<IEntityTransformationService<Partner>> partnerTransformationService;

    public DataTransformationServiceTests(EntityDataFixture dataFixture)
    {
        this.logger = new Mock<ILogger<DataTransformationService>>();
        this.dataLoaderService = new Mock<IDataLoaderService>();
        this.partnerTransformationService = new Mock<IEntityTransformationService<Partner>>();

        this.dataFixture = dataFixture;
    }

    [Fact()]
    public void DataTransformationService_CanBeCreated()
    {
        var service = CreateServiceInstance();
        service.Should().NotBeNull();
    }

    [Fact()]
    public async Task MergeAndSummarizePartnerDataAsync_ShouldSummarize_WhenSingleInput()
    {
        dataLoaderService.Setup(m => m
            .LoadFromDataStoreAsync<Partner>(It.IsAny<IDataStore>()))
            .ReturnsAsync(dataFixture.CreatePartner());

        var inputDataStores = new[] { new Mock<IDataStore>().Object };
        var outputDataStore = new Mock<IDataStore>();

        var service = CreateServiceInstance();
        await service.MergeAndSummarizePartnerDataAsync(inputDataStores, outputDataStore.Object);

        dataLoaderService.Verify(m => m.SaveToDataStoreAsync(It.IsAny<IDataStore>(), It.IsAny<IEnumerable<DeviceMeasurementSummary>>()));
    }

    [Fact()]
    public async Task MergeAndSummarizePartnerDataAsync_ShouldMerge_WhenDuplicateInput()
    {
        var deviceCount = 2;
        var partnerData = dataFixture.CreatePartner(partnerId: 1, deviceCount, sensorCount: 2);

        dataLoaderService.Setup(m => m
            .LoadFromDataStoreAsync<Partner>(It.IsAny<IDataStore>()))
            .ReturnsAsync(partnerData);

        partnerTransformationService.Setup(m => m
            .Merge(It.IsAny<IEnumerable<Partner>>()))
            .Returns((IEnumerable<Partner> partners) => partners.Take(1));

        var inputDataStores = new[] { new Mock<IDataStore>().Object, new Mock<IDataStore>().Object };
        var outputDataStore = new Mock<IDataStore>();

        var service = CreateServiceInstance();
        await service.MergeAndSummarizePartnerDataAsync(inputDataStores, outputDataStore.Object);

        partnerTransformationService.Verify(m => m.Merge(It.IsAny<IEnumerable<Partner>>()));
        dataLoaderService.Verify(m => m.SaveToDataStoreAsync(It.IsAny<IDataStore>(), It.Is<IEnumerable<DeviceMeasurementSummary>>(_ => _.Count() == deviceCount)));
    }

    [Fact()]
    public async Task MergeAndSummarizePartnerDataAsync_ShouldPass_WhenEmptyInput()
    {
        var deviceCount = 2;
        var partnerData = dataFixture.CreatePartner(partnerId: 1, deviceCount, sensorCount: 2);

        var inputDataStores = new IDataStore[] { };
        var outputDataStore = new Mock<IDataStore>();

        var service = CreateServiceInstance();
        await service.MergeAndSummarizePartnerDataAsync(inputDataStores, outputDataStore.Object);

        dataLoaderService.Verify(m => m.SaveToDataStoreAsync(It.IsAny<IDataStore>(), It.Is<IEnumerable<DeviceMeasurementSummary>>(_ => _.Count() == 0)));
    }

    [Fact()]
    public async Task MergeAndSummarizePartnerDataAsync_ShoulThrow_WhenNullInput()
    {
        var outputDataStore = new Mock<IDataStore>();

        var service = CreateServiceInstance();

        await FluentActions.Invoking(async () => await service.MergeAndSummarizePartnerDataAsync(null, outputDataStore.Object))
            .Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact()]
    public async Task MergeAndSummarizePartnerDataAsync_ShouldMatchSummary_WhenKnownInput()
    {
        var (partner, expectedSummary) = dataFixture.PartnerWithKnownSummary;
        var actualSummary = Enumerable.Empty<DeviceMeasurementSummary>();

        dataLoaderService.Setup(m => m
            .LoadFromDataStoreAsync<Partner>(It.IsAny<IDataStore>()))
            .ReturnsAsync(partner);

        dataLoaderService.Setup(m => m
            .SaveToDataStoreAsync(It.IsAny<IDataStore>(), It.IsAny<IEnumerable<DeviceMeasurementSummary>>()))
            .Callback<IDataStore, object>((dataStore, summary) => actualSummary = (IEnumerable<DeviceMeasurementSummary>)summary)
            .Returns(Task.FromResult(partner));

        var inputDataStores = new[] { new Mock<IDataStore>().Object };
        var outputDataStore = new Mock<IDataStore>();

        var service = CreateServiceInstance();
        await service.MergeAndSummarizePartnerDataAsync(inputDataStores, outputDataStore.Object);
        
        dataLoaderService.Verify(m => m.SaveToDataStoreAsync(It.IsAny<IDataStore>(),
            It.IsAny<IEnumerable<DeviceMeasurementSummary>>()));
        actualSummary.Should().BeEquivalentTo(expectedSummary);
    }

    private DataTransformationService CreateServiceInstance()
    {
        return new DataTransformationService(logger.Object, dataLoaderService.Object, partnerTransformationService.Object);
    }

    private static bool VerifyFluentAssertion(Action assertion)
    {
        using var assertionScope = new AssertionScope();
        assertion?.Invoke();
        return !assertionScope.Discard().Any();
    }
}