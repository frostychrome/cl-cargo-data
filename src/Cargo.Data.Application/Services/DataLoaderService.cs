using AutoMapper;
using Cargo.Data.Application.Interfaces.DataHandlers;

namespace Cargo.Data.Application.Services;

public class DataLoaderService: IDataLoaderService
{
    private readonly IMapper mapper;

    public DataLoaderService(IMapper mapper)
    {
        this.mapper = mapper;
    }

    public async Task<TResult> LoadFromDataStoreAsync<TResult>(IDataStore dataStore)
    {
        var data = await dataStore.ReadDataAsync();
        return mapper.Map<TResult>(data);
    }

    public async Task SaveToDataStoreAsync(IDataStore outputDataStore, object data)
    {
        await outputDataStore.WriteDataAsync(data);
    }
}
