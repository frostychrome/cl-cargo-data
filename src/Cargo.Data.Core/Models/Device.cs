using Cargo.Data.Core.Models.Base;

namespace Cargo.Data.Core.Models;

public class Device: BaseEntity
{
    public int? Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTimeOffset? ActivationDtm { get; set; }

    public ICollection<BaseSensor>? Sensors { get; set; }
}
