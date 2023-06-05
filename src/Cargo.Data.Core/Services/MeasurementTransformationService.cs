namespace Cargo.Data.Core.Services;

public class MeasurementTransformationService : IEntityTransformationService<Measurement>
{
    private readonly ILogger<MeasurementTransformationService> logger;

    public MeasurementTransformationService(ILogger<MeasurementTransformationService> logger)
    {
        this.logger = logger;
    }

    public IEnumerable<Measurement> Merge(IEnumerable<Measurement> measurements)
    {
        var mergedMeasurements =
            from measurement in measurements ?? throw new ArgumentNullException("Measurements cannot be null")
            group measurement by measurement.Timestamp into measurementGroup
            select new Measurement
            {
                Timestamp = measurementGroup.Key,
                Value = measurementGroup.Count() <= 1 || measurementGroup.All(m => m.Value == measurementGroup.First().Value)
                    ? measurementGroup.First().Value
                    : throw new ApplicationException($"Cannot merge sensor measurements due to conflicting data points at timestamp: {measurementGroup.Key}")
            };
        return mergedMeasurements;
    }
}