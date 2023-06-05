using System.Text.Json.Serialization;

namespace Cargo.Data.Application.Models.RequestDto.Foo2;

public class Foo2Document : DocumentRoot
{
    public int CompanyId { get; set; }

    [JsonPropertyName("Company")]
    public string? CompanyName { get; set; }

    public Device[]? Devices { get; set; }
}
