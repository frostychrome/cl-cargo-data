using Cargo.Data.Application.Interfaces;
using Cargo.Data.Application.Interfaces.DataHandlers;
using Cargo.Data.Application.Models.ResponseDto;
using Cargo.Data.Core.Enums;
using Cargo.Data.Core.Interfaces;
using Cargo.Data.Core.Models;
using Cargo.Data.Core.Models.Base;

namespace Cargo.Data.Application.Services;

public class DataTransformationService : IDataTransformationService
{
    private readonly ILogger<DataTransformationService> logger;
    private readonly IDataLoaderService dataLoaderService;
    private readonly IEntityTransformationService<Partner> partnerTransformationService;

    public DataTransformationService(ILogger<DataTransformationService> logger,
        IDataLoaderService dataLoaderService,
        IEntityTransformationService<Partner> partnerTransformationService)
    {
        this.logger = logger;
        this.dataLoaderService = dataLoaderService;
        this.partnerTransformationService = partnerTransformationService;
    }

    public async Task MergeAndSummarizePartnerDataAsync(IEnumerable<IDataStore> partnerDataStores, IDataStore summaryDestination)
    {
        var partnerData = new List<Partner>();
        foreach (var partnerDataStore in partnerDataStores ?? throw new ArgumentNullException($"{nameof(partnerDataStores)} must not be null"))
        {
            var partnerDataRecord = await dataLoaderService.LoadFromDataStoreAsync<Partner>(partnerDataStore);
            partnerData.Add(partnerDataRecord);
        }

        if(partnerData.Count > 1)
        {
            logger.LogInformation("Merging data from {count} sources", partnerData.Count);
            partnerData = partnerTransformationService.Merge(partnerData).ToList();
        }

        var summaries = SummarizeMeasurementsByPartnerDevices(partnerData).ToList();

        await dataLoaderService.SaveToDataStoreAsync(summaryDestination, summaries);
    }

    private IEnumerable<DeviceMeasurementSummary> SummarizeMeasurementsByPartnerDevices(IEnumerable<Partner> partners)
    {
        var partnerDevices = from partner in partners ?? Enumerable.Empty<Partner>()
                             from device in partner?.Devices ?? Enumerable.Empty<Device>()
                             select (Partner: partner, Device: device);

        foreach (var partnerDevice in partnerDevices)
        {
            var summary = new DeviceMeasurementSummary
            {
                CompanyId = partnerDevice.Partner.Id,
                CompanyName = partnerDevice.Partner.Name,
                DeviceId = partnerDevice.Device.Id,
                DeviceName = partnerDevice.Device.Name,
            };
            EnrichDeviceSummaryWithCalculatedAttributes(summary, partnerDevice.Device);

            yield return summary;
        }
    }

    private void EnrichDeviceSummaryWithCalculatedAttributes(DeviceMeasurementSummary summary, Device device)
    {
        var sortedMeasurements = from sensor in device?.Sensors ?? Enumerable.Empty<BaseSensor>()
                                 from measurement in sensor.Measurements ?? Enumerable.Empty<Measurement>()
                                 orderby measurement.Timestamp
                                 select (Measurement: measurement, sensor.Type);

        foreach (var sensorMeasurement in sortedMeasurements)
        {
            summary.FirstReadingDtm ??= sensorMeasurement.Measurement.Timestamp;
            summary.LastReadingDtm = sensorMeasurement.Measurement.Timestamp;

            switch (sensorMeasurement.Type)
            {
                case SensorType.Temperature:
                    summary.TemperatureCount = (summary.TemperatureCount ?? 0) + 1;

                    var avgTemperature = summary.AverageTemperature ?? 0;
                    summary.AverageTemperature = avgTemperature +
                        (sensorMeasurement.Measurement.Value - avgTemperature) / summary.TemperatureCount;
                    break;
                case SensorType.Humidity:
                    summary.HumidityCount = (summary.HumidityCount ?? 0) + 1;

                    var avgHumidity = summary.AverageHumidity ?? 0;
                    summary.AverageHumidity = avgHumidity +
                        (sensorMeasurement.Measurement.Value - avgHumidity) / summary.HumidityCount;
                    break;
            }
        }
    }
}
