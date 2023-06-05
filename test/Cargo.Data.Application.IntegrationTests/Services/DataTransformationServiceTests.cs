using Cargo.Data.Application.Interfaces;
using Cargo.Data.Application.Interfaces.DataHandlers;
using Cargo.Data.Application.Models.RequestDto.Foo1;
using Cargo.Data.Application.Models.RequestDto.Foo2;
using Cargo.Data.Application.Models.ResponseDto;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cargo.Data.Application.IntegrationTests.Services;

public class DataTransformationServiceTests: IClassFixture<BaseTest>
{
    private readonly IHost Host;

    public DataTransformationServiceTests(BaseTest baseTest)
    {
        Host = baseTest.TestHost;
    }

    [Fact()]
    public async Task MergeAndSummarizeTest()
    {
        var outputFilePath = @"SampleData\DeviceMeasurementSummary.json";
        File.Delete(outputFilePath);

        var dataStoreFactory = Host.Services.GetService<IDataStoreFactory>() ?? throw new Exception($"Could not resolve {nameof(IDataStoreFactory)}");
        var dataTransformationService = Host.Services.GetService<IDataTransformationService>() ?? throw new Exception($"Could not resolve {nameof(IDataTransformationService)}");

        var dataStores = new[]
            {
                dataStoreFactory.CreateLocalJsonFileDataStore<Foo1Document>(@"SampleData\DeviceDataFoo1.json"),
                dataStoreFactory.CreateLocalJsonFileDataStore<Foo2Document>(@"SampleData\DeviceDataFoo2.json"),
            };

        var summaryDataStore = dataStoreFactory.CreateLocalJsonFileDataStore<DeviceMeasurementSummary>(outputFilePath);

        await dataTransformationService.MergeAndSummarizePartnerDataAsync(dataStores, summaryDataStore);

        Assert.True(File.Exists(outputFilePath));
    }
}
