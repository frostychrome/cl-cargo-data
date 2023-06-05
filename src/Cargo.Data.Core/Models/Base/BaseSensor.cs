namespace Cargo.Data.Core.Models.Base;

public abstract class BaseSensor: BaseEntity
{
    public int? Id { get; set; }

    public abstract SensorType Type { get; }

    public ICollection<Measurement>? Measurements { get; set; }
}
