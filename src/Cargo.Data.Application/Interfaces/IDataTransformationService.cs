using Cargo.Data.Application.Interfaces.DataHandlers;

namespace Cargo.Data.Application.Interfaces;

public interface IDataTransformationService
{
    /// <summary>
    /// Merges and summarizes partner data feeds from multiple data sources.
    /// </summary>
    /// <param name="partnerDataSources">Input data sources.</param>
    /// <param name="summaryDestination">Destination location for summarized data.</param>
    Task MergeAndSummarizePartnerDataAsync(IEnumerable<IDataStore> partnerDataSources, IDataStore summaryDestination);
}