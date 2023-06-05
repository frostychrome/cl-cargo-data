namespace Cargo.Data.Application.Models.RequestDto.Foo1;

public class Tracker
{
    public int Id { get; set; }

    public string? Model { get; set; }

    public DateTimeOffset? ShipmentStartDtm { get; set; }

    public Sensor[]? Sensors { get; set; }
}