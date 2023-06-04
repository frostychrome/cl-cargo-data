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
        Assert.True(false, "This test needs an implementation");
    }
}
