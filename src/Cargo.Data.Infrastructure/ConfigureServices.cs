using Cargo.Data.Application.Interfaces.DataHandlers;
using Cargo.Data.Application.Models.RequestDto.Foo1;
using Cargo.Data.Application.Models.RequestDto.Foo2;
using Cargo.Data.Application.Services;
using Cargo.Data.Infrastructure.DataHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Cargo.Data.Infrastructure;

public static class ConfigureServices
{
    public static void RegisterInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IDataStoreFactory, DataStoreFactory>();
        services.AddTransient<IDataStore, LocalJsonFileDataStore<Foo1Document>>();
        services.AddTransient<IDataStore, LocalJsonFileDataStore<Foo2Document>>();
    }
}
