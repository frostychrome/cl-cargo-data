using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Cargo.Data.Infrastructure;

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
                services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                services.RegisterApplicationServices();
                services.RegisterInfrastructureServices();
            })
            .Build();
    }
}