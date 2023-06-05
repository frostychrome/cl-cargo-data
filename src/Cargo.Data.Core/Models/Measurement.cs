using Cargo.Data.Core.Models.Base;

namespace Cargo.Data.Core.Models;

public class Measurement : BaseEntity
{
    public DateTimeOffset Timestamp { get; set; }

    public double Value { get; set; }
}