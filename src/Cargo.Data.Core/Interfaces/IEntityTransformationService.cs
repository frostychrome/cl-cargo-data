using Cargo.Data.Core.Models.Base;

namespace Cargo.Data.Core.Interfaces;

public interface IEntityTransformationService<T> where T : BaseEntity
{
    IEnumerable<T> Merge(IEnumerable<T> entities);
}
