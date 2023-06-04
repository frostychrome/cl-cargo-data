namespace Cargo.Data.Application.Models.ResponseDto;

public class DeviceMeasurementSummary
{
    public int CompanyId { get; set; }

    public string? CompanyName { get; set; }

    public int DeviceId { get; set; }

    public string? DeviceName { get; set; }

    public DateTimeOffset? FirstReadingDtm { get; set; }

    public DateTimeOffset? LastReadingDtm { get; set; }

    public int? TemperatureCount { get; set; }

    public double? AverageTemperature { get; set; }

    public int? HumidityCount { get; set; }

    public double? AverageHumidity { get; set; }
}
