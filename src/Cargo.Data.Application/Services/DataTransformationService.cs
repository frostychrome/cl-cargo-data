using Cargo.Data.Application.Interfaces;
using Cargo.Data.Core.Interfaces;
using Cargo.Data.Core.Models;

namespace Cargo.Data.Application.Services;
public class DataTransformationService : IDataTransformationService
{
    private readonly ILogger<DataTransformationService> logger;
    private readonly IEntityTransformationService<Partner> partnerTransformationService;

    public DataTransformationService(ILogger<DataTransformationService> logger, IEntityTransformationService<Partner> partnerTransformationService)
    {
        this.logger = logger;
        this.partnerTransformationService = partnerTransformationService;
    }

    public void MergeAndSummarize(string source1, string source2, string output)
    {
        logger.LogInformation("Merging data from {source1} and {source2}", source1, source2);

        throw new NotImplementedException();
    }
}
