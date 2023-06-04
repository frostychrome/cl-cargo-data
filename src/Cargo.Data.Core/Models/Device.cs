namespace Cargo.Data.Core.Models;

public class Device
{
    public int? Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTimeOffset? ActivationDtm { get; set; }

    public ICollection<Sensor>? Sensors { get; set; }
}
