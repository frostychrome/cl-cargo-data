namespace Cargo.Data.Core.Services;

public class SensorTransformationService : IEntityTransformationService<BaseSensor>
{
    private readonly ILogger<SensorTransformationService> logger;
    private readonly ISensorFactory sensorFactory;
    private readonly IEntityTransformationService<Measurement> measurementTransformationService;

    public SensorTransformationService(ILogger<SensorTransformationService> logger,
        ISensorFactory sensorFactory,
        IEntityTransformationService<Measurement> measurementTransformationService)
    {
        this.logger = logger;
        this.sensorFactory = sensorFactory;
        this.measurementTransformationService = measurementTransformationService;
    }

    public IEnumerable<BaseSensor> Merge(IEnumerable<BaseSensor> sensors)
    {
        var mergedSensors =
            from sensor in sensors
            group sensor by (sensor.Type, sensor.Id) into sensorGroup
            select sensorFactory.CreateSensor(sensorGroup.Key.Type, opt =>
            {
                opt.Id = sensorGroup.Key.Id;
                opt.Measurements = measurementTransformationService.Merge(sensorGroup
                    .SelectMany(sensor => sensor.Measurements ?? Enumerable.Empty<Measurement>())).ToList();
            });
        return mergedSensors;
    }
}
