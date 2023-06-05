using Cargo.Data.Application.Interfaces.DataHandlers;
using Microsoft.Extensions.Logging;

namespace Cargo.Data.Infrastructure.DataHandlers;

public class DataStoreFactory : IDataStoreFactory
{
    private readonly ILogger<DataStoreFactory> logger;

    public DataStoreFactory(ILogger<DataStoreFactory> logger)
    {
        this.logger = logger;
    }

    public IDataStore CreateLocalJsonFileDataStore<T>(string filePath)
    {
        return new LocalJsonFileDataStore<T>(filePath);
    }
}