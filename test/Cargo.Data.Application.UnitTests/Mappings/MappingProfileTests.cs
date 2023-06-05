using AutoMapper;
using Cargo.Data.Application.MapProfiles;
using Cargo.Data.Core.Models.Base;
using System.Runtime.Serialization;
using Foo1Dto = Cargo.Data.Application.Models.RequestDto.Foo1;
using Foo2Dto = Cargo.Data.Application.Models.RequestDto.Foo2;
using Model = Cargo.Data.Core.Models;

namespace Cargo.Data.Application.UnitTests.Mappings;

public class MappingProfileTests
{
    private readonly IConfigurationProvider configuration;
    private readonly IMapper mapper;

    public MappingProfileTests()
    {
        this.configuration = new MapperConfiguration(configuration =>
            {
                configuration.AddProfile(new Foo1DtoProfile());
                configuration.AddProfile(new Foo2DtoProfile());
            });
        this.mapper = configuration.CreateMapper();
    }

    [Fact]
    public void ShouldHaveValidConfiguration()
    {
        configuration.AssertConfigurationIsValid();
    }

    [Theory]
    [InlineData(typeof(Foo1Dto.Foo1Document), typeof(Model.Partner))]
    [InlineData(typeof(Foo1Dto.Tracker), typeof(Model.Device))]
    [InlineData(typeof(Foo1Dto.Crumb), typeof(Model.Measurement))]
    [InlineData(typeof(Foo2Dto.Foo2Document), typeof(Model.Partner))]
    [InlineData(typeof(Foo2Dto.Device), typeof(Model.Device))]
    [InlineData(typeof(Foo2Dto.SensorData), typeof(Model.Measurement))]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = Activator.CreateInstance(source);

        mapper.Map(instance, source, destination);
    }
}