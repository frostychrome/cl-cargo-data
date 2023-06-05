namespace Cargo.Data.Core.Services;

public class PartnerTransformationService : IEntityTransformationService<Partner>
{
    private readonly ILogger<PartnerTransformationService> logger;
    private readonly IEntityTransformationService<Device> deviceTransformationService;

    public PartnerTransformationService(ILogger<PartnerTransformationService> logger, IEntityTransformationService<Device> deviceTransformationService)
    {
        this.logger = logger;
        this.deviceTransformationService = deviceTransformationService;
    }

    public IEnumerable<Partner> Merge(IEnumerable<Partner> partners)
    {
        var mergedPartners =
            from partner in partners
            group partner by partner.Id into partnerGroup
            select new Partner
            {
                Id = partnerGroup.Key,
                Name = partnerGroup.First().Name,
                Devices = deviceTransformationService.Merge(partnerGroup.SelectMany(partner => partner.Devices ?? Enumerable.Empty<Device>())).ToList(),
            };
        return mergedPartners;
    }
}
