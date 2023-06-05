using Cargo.Data.Application.Interfaces.DataHandlers;
using Cargo.Data.Infrastructure.Converters;
using System.Text.Json;

namespace Cargo.Data.Infrastructure.DataHandlers;

public class LocalJsonFileDataStore<T> : IDataStore
{
    private readonly JsonSerializerOptions jsonSerializerOptions =
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = default,
            WriteIndented = true,
            Converters = { new DateTimeOffsetConverter() }
        };

    private readonly string filePath;

    public LocalJsonFileDataStore(string filePath)
    {
        this.filePath = filePath;
    }

    public async Task<object> ReadDataAsync()
    {
        using var fileStream = File.OpenRead(filePath) ?? throw new ApplicationException($"Could not open local file '{filePath}'");
        return await JsonSerializer.DeserializeAsync<T>(fileStream, jsonSerializerOptions)
            ?? throw new ApplicationException($"Could not read local JSON file '{filePath}'");
    }

    public async Task WriteDataAsync(object data)
    {
        using var fileStream = File.OpenWrite(filePath) ?? throw new ApplicationException($"Could not open local file '{filePath}'");
        await JsonSerializer.SerializeAsync(fileStream, data, jsonSerializerOptions);
    }
}
