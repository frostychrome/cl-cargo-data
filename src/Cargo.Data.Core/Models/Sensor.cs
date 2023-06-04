namespace Cargo.Data.Core.Models;

public abstract class Sensor
{
    public int? Id { get; set; }

    public ICollection<Measurement>? Measurements { get; set; }
}
