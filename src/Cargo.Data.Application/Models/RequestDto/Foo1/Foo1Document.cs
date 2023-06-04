namespace Cargo.Data.Application.Models.RequestDto.Foo1;

public class Foo1Document : DocumentRoot
{
    public int PartnerId { get; set; }

    public string PartnerName { get; set; } = string.Empty;

    public Tracker[]? Trackers { get; set; }
}