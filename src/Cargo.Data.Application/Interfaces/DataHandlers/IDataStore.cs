namespace Cargo.Data.Application.Interfaces.DataHandlers;

public interface IDataStore
{
    Task<object> ReadDataAsync();

    Task WriteDataAsync(object data);
}
