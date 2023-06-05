namespace Cargo.Data.Application.Interfaces.DataHandlers;

public interface IDataStoreFactory
{
    IDataStore CreateLocalJsonFileDataStore<T>(string filePath);
}
