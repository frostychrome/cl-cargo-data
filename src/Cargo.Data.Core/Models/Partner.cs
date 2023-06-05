using Cargo.Data.Core.Models.Base;

namespace Cargo.Data.Core.Models;

public class Partner : BaseEntity
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public ICollection<Device>? Devices { get; set; }
}
