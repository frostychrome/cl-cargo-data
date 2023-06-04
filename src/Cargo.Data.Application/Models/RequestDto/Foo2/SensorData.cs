namespace Cargo.Data.Application.Models.RequestDto.Foo2;

public class SensorData
{
    public string SensorType { get; set; } = string.Empty;

    public DateTimeOffset? DateTime { get; set; }
    
    public float Value { get; set; }
}
