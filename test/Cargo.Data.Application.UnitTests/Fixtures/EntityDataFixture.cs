using Cargo.Data.Application.Models.ResponseDto;
using Cargo.Data.Core.Models;
using Cargo.Data.Core.Models.Base;

namespace Cargo.Data.Application.UnitTests.Fixtures;

public class EntityDataFixture
{
    public (Partner Partner, IEnumerable<DeviceMeasurementSummary> Summary) PartnerWithKnownSummary => CreatePartnerWithKnownSummary();

    public Partner CreatePartner(int partnerId = 1, int deviceCount = 2, int sensorCount = 2, int measurementCount = 3)
    {
        var partner = new Partner
        {
            Id = partnerId,
            Name = $"Partner {partnerId}",
            Devices = CreateDevices(partnerId * 100, deviceCount, sensorCount, measurementCount),
        };
        return partner;
    }

    public Device[] CreateDevices(int deviceId = 1, int deviceCount = 2, int sensorCount = 2, int measurementCount = 3)
    {
        var devices = new List<Device>();
        for (int i = 0; i < deviceCount; i++)
        {
            var device = new Device
            {
                Id = deviceId,
                Name = $"Device {deviceId}",
                ActivationDtm = DateTime.UtcNow,
                Sensors = CreateSensors(deviceId * 100, sensorCount, measurementCount),
            };
            devices.Add(device);
            deviceId++;
        }
        return devices.ToArray();
    }

    public BaseSensor[] CreateSensors(int sensorId = 1, int sensorCount = 2, int measurementCount = 3)
    {
        var sensors = new List<BaseSensor>();
        for (int i = 0; i < sensorCount; i++)
        {
            BaseSensor sensor;
            if (i % 2 == 0)
            {
                sensor = new TemperatureSensor
                {
                    Id = sensorId,
                    Measurements = CreateMeasurement(measurementCount),
                };
            }
            else
            {
                sensor = new HumiditySensor
                {
                    Id = sensorId,
                    Measurements = CreateMeasurement(measurementCount),
                };
                sensors.Add(sensor);
                sensorId++;
            }
        }
        return sensors.ToArray();
    }

    public Measurement[] CreateMeasurement(int measurementCount = 3)
    {
        var measurements = new List<Measurement>();
        for (int i = 0; i < measurementCount; i++)
        {
            var measurement = new Measurement
            {
                Timestamp = DateTime.UtcNow,
                Value = 10d,
            };
            measurements.Add(measurement);
        }
        return measurements.ToArray();
    }

    private (Partner Partner, IEnumerable<DeviceMeasurementSummary> Summary) CreatePartnerWithKnownSummary()
    {
        var partner = new Partner
        {
            Id = 1,
            Name = $"Partner 1",
            Devices = new[]
            {
                new Device
                {
                    Id = 1,
                    Name = $"Device 1",
                    ActivationDtm = new DateTime(2010, 1, 1, 0, 0, 0),
                    Sensors = new BaseSensor[]
                    {
                        new TemperatureSensor
                        {
                            Id = 1,
                            Measurements = new Measurement[]
                            {
                                new Measurement
                                {
                                    Timestamp = new DateTime(2010, 1, 1, 9, 0, 0),
                                    Value = 10d,
                                },
                                new Measurement
                                {
                                    Timestamp = new DateTime(2010, 1, 1, 10, 0, 0),
                                    Value = 20d,
                                },
                                new Measurement
                                {
                                    Timestamp = new DateTime(2010, 1, 1, 11, 0, 0),
                                    Value = 30d,
                                },
                            },
                        },
                        new HumiditySensor
                        {
                            Id = 1,
                            Measurements = new Measurement[]
                            {
                                new Measurement
                                {
                                    Timestamp = new DateTime(2010, 1, 1, 9, 0, 0),
                                    Value = 50d,
                                },
                                new Measurement
                                {
                                    Timestamp = new DateTime(2010, 1, 1, 10, 0, 0),
                                    Value = 60d,
                                },
                                new Measurement
                                {
                                    Timestamp = new DateTime(2010, 1, 1, 11, 0, 0),
                                    Value = 70d,
                                },
                            },
                        },
                    },
                },
            },
        };

        var summary = new DeviceMeasurementSummary
        {
            CompanyId = 1,
            CompanyName = $"Partner 1",
            DeviceId = 1,
            DeviceName = $"Device 1",
            FirstReadingDtm = new DateTime(2010, 1, 1, 9, 0, 0),
            LastReadingDtm = new DateTime(2010, 1, 1, 11, 0, 0),
            TemperatureCount = 3,
            AverageTemperature = 20d,
            HumidityCount = 3,
            AverageHumidity = 60d,
        };
        return (partner, new[] { summary });
    }
}
