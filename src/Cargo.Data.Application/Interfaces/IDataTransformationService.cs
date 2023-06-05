namespace Cargo.Data.Application.Interfaces;

public interface IDataTransformationService
{
    void MergeAndSummarize(string source1, string source2, string output);
}