namespace Cargo.Data.Application.Models.RequestDto.Foo2;

public class Device
{
    public int DeviceID { get; set; }

    public string Name { get; set; } = string.Empty;
    
    public DateTimeOffset StartDateTime { get; set; }
    
    public SensorData[]? SensorData { get; set; }
}
