namespace Cargo.Data.Application.Interfaces.DataHandlers;

public interface IDataLoaderService
{
    Task<TResult> LoadFromDataStoreAsync<TResult>(IDataStore dataStore);

    Task SaveToDataStoreAsync(IDataStore outputDataStore, object data);
}