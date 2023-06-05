using Cargo.Data.Core.Models;
using Cargo.Data.Core.Models.Base;

namespace Cargo.Data.Core.UnitTests.Fixtures;

public class EntityFixture
{
    public List<Partner> Partners= CreatePartners(2);
    public List<Device> Devices= CreateDevices(4);
    public List<BaseSensor> Sensors = CreateSensors(2);
    public List<Measurement> Measurements = CreateMeasurements(5);
    
    private static List<Measurement> CreateMeasurements(int count)
    {
        var measurements = new List<Measurement>(count);
        
        var timestamp = new DateTime(2010, 1, 1, 0, 0, 0);
        for (int i = 0; i < count; i++)
        {
            measurements.Add(new Measurement
            {
                Timestamp = timestamp.AddYears(i),
                Value = i
            });
        }
        return measurements;
    }

    private static List<BaseSensor> CreateSensors(int count)
    {
        var sensors = new List<BaseSensor>(count);

        for (int i = 0; i < count; i++)
        {
            BaseSensor sensor = (i % 2) switch
            {
                0 => new TemperatureSensor(),
                1 => new HumiditySensor(),
                _ => throw new NotImplementedException()
            };
            sensor.Id = i;
            sensors.Add(sensor);
        }
        return sensors;
    }

    private static List<Device> CreateDevices(int count)
    {
        var devices = new List<Device>(count);

        for (int i = 0; i < count; i++)
        {
            devices.Add(new Device
            {
                Id = i,
                Name = $"Device{i}",
                ActivationDtm = new DateTime(2010, 1, 1, 0, 0, 0).AddYears(i),
                Sensors = CreateSensors(2)
            });
        }
        return devices;
    }

    private static List<Partner> CreatePartners(int count)
    {
        var partners = new List<Partner>(count);

        for (int i = 0; i < count; i++)
        {
            partners.Add(new Partner
            {
                Id = i,
                Name = $"Partner{i}",
                Devices = CreateDevices(1)
            });
        }
        return partners;
    }
}
