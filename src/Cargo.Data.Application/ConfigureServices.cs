using Cargo.Data.Application.Interfaces;
using Cargo.Data.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Cargo.Data.Application;
public static class ConfigureServices
{
    public static void RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDataTransformationService, DataTransformationService>();
    }
}
