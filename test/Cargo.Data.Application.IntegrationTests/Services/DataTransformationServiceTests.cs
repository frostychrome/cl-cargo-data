using Cargo.Data.Application.Interfaces;
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
    public async void MergeAndSummarizeTest()
    {
        File.Delete("merged.json");

        var dataTransformationService = Host.Services.GetService<IDataTransformationService>();
        dataTransformationService?.MergeAndSummarize("foo1.json", "foo2.json", "merged.json");

        Assert.True(File.Exists("merged.json"));
    }
}
