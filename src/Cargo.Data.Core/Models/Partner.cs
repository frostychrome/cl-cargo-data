namespace Cargo.Data.Core.Models;

public class Partner
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public ICollection<Device>? Devices { get; set; }
}
