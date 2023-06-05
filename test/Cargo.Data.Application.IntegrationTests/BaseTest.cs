using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cargo.Data.Application.IntegrationTests;

public class BaseTest
{
    public IHost TestHost { get; }

    public BaseTest()
    {
        TestHost = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddLogging();
                services.AddOptions();
                services.RegisterApplicationServices();
            })
            .Build();
    }
}