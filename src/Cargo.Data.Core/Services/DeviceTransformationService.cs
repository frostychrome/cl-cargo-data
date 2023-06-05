namespace Cargo.Data.Core.Services;

public class DeviceTransformationService : IEntityTransformationService<Device>
{
    private readonly ILogger<DeviceTransformationService> logger;
    private readonly IEntityTransformationService<BaseSensor> sensorTransformationService;

    public DeviceTransformationService(ILogger<DeviceTransformationService> logger, IEntityTransformationService<BaseSensor> sensorTransformationService)
    {
        this.logger = logger;
        this.sensorTransformationService = sensorTransformationService;
    }

    public IEnumerable<Device> Merge(IEnumerable<Device> devices)
    {
        var mergedDevices =
            from device in devices
            group device by device.Id into deviceGroup
            select new Device
            {
                Id = deviceGroup.Key,
                Name = deviceGroup.First().Name,
                ActivationDtm = deviceGroup.First().ActivationDtm,
                Sensors = sensorTransformationService.Merge(deviceGroup.SelectMany(device => device.Sensors ?? Enumerable.Empty<BaseSensor>())).ToList()
            };
        return mergedDevices;
    }
}
