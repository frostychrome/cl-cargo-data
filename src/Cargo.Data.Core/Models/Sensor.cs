using Cargo.Data.Core.Enums;

namespace Cargo.Data.Core.Models;

public class Sensor
{
    public int? Id { get; set; }

    public SensorType Type { get; set; }

    public ICollection<Measurement>? Measurements { get; set; }
}
