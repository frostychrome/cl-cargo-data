using AutoMapper;
using Cargo.Data.Core.Enums;
using Cargo.Data.Application.Models.RequestDto;
using Foo1Dto = Cargo.Data.Application.Models.RequestDto.Foo1;
using Model = Cargo.Data.Core.Models;

namespace Cargo.Data.Application.MapProfiles;

public class Foo1DtoProfile : Profile
{
    public Foo1DtoProfile()
    {
        CreateDocumentProfile();
        CreateDeviceProfile();
        CreateSensorProfile();
        CreateCrumbProfile();
    }

    private void CreateDocumentProfile()
    {
        CreateMap<Foo1Dto.Foo1Document, Model.Partner>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PartnerId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PartnerName))
            .ForMember(dest => dest.Devices, opt => opt.MapFrom(src => src.Trackers))
            .ReverseMap();
    }

    private void CreateDeviceProfile()
    {
        CreateMap<Foo1Dto.Tracker, Model.Device>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Model))
            .ForMember(dest => dest.ActivationDtm, opt => opt.MapFrom(src => src.ShipmentStartDtm))
            .ReverseMap();
    }

    private void CreateSensorProfile()
    {
        CreateMap<Foo1Dto.Sensor, Model.Sensor>()
            .Include<Foo1Dto.Sensor, Model.TemperatureSensor>()
            .Include<Foo1Dto.Sensor, Model.HumiditySensor>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Measurements, opt => opt.MapFrom(src => src.Crumbs))
            .ConstructUsing((src, ctx) => SensorTypeMap.MapFoo1SensorType(src.Name ?? string.Empty) switch
                {
                    SensorType.Temperature => ctx.Mapper.Map(src, new Model.TemperatureSensor { }),
                    SensorType.Humidity => ctx.Mapper.Map(src, new Model.HumiditySensor { }),
                    _ => throw new ApplicationException($"Unsupported sensor type: {src.Name}"),
                })
            .ReverseMap();

        CreateMap<Foo1Dto.Sensor, Model.TemperatureSensor>().ReverseMap();
        CreateMap<Foo1Dto.Sensor, Model.HumiditySensor>().ReverseMap();
    }

    private void CreateCrumbProfile()
    {
        CreateMap<Foo1Dto.Crumb, Model.Measurement>()
            .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.CreatedDtm))
            .ReverseMap();
    }
}
