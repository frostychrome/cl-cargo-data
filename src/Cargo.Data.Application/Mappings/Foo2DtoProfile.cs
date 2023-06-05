using AutoMapper;
using Cargo.Data.Application.Models.RequestDto;
using Cargo.Data.Core.Enums;
using Cargo.Data.Core.Models.Base;
using Foo2Dto = Cargo.Data.Application.Models.RequestDto.Foo2;
using Model = Cargo.Data.Core.Models;

namespace Cargo.Data.Application.MapProfiles;

public class Foo2DtoProfile : Profile
{
    public Foo2DtoProfile()
    {
        CreateDocumentProfile();
        CreateDeviceProfile();
        CreateSensorDataProfile();
    }

    private void CreateDocumentProfile()
    {
        CreateMap<Foo2Dto.Foo2Document, Model.Partner>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CompanyId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CompanyName))
            .ReverseMap();
    }

    private void CreateDeviceProfile()
    {
        CreateMap<Foo2Dto.Device, Model.Device>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DeviceID))
            .ForMember(dest => dest.ActivationDtm, opt => 
                opt.MapFrom(src => src.StartDateTime))
            .ForMember(dest => dest.Sensors, opt => 
                opt.MapFrom((src, dest, idx, ctx) => GroupSensorDataBySensorType(src.SensorData, ctx)))
            .ReverseMap();
    }

    private void CreateSensorDataProfile()
    {
        CreateMap<Foo2Dto.SensorData, Model.Measurement>()
            .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.DateTime))
            .ForSourceMember(src => src.SensorType, opt => opt.DoNotValidate())
            .ReverseMap();
    }

    private IEnumerable<BaseSensor> GroupSensorDataBySensorType(
        IEnumerable<Foo2Dto.SensorData>? sensorDataRecords,
        ResolutionContext ctx)
    {
        if (sensorDataRecords is null)
            yield break;

        var sensorsByType = sensorDataRecords.GroupBy(x => x.SensorType);
        foreach (var sensorTypeGroup in sensorsByType)
        {
            var sensorType = SensorTypeMap.MapFoo2SensorType(sensorTypeGroup.Key);
            var sensorMeasurements = ctx.Mapper.Map<ICollection<Model.Measurement>>(sensorTypeGroup);
            BaseSensor sensor = sensorType switch
            {
                SensorType.Temperature => new Model.TemperatureSensor { Measurements = sensorMeasurements, },
                SensorType.Humidity => new Model.HumiditySensor { Measurements = sensorMeasurements, },
                _ => throw new ApplicationException($"Unsupported sensor type: {sensorTypeGroup.Key}"),
            };
            yield return sensor;
        }
    }
}
