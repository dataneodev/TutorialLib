using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TutorialsLib.SharedKernel.Specification;
using TutorialsLib.Core.Entities;

namespace TutorialsLib.Core.Interfaces
{
    public interface IAsyncRepository<T> where T : BaseEntity, IAggregateRoot
    {
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> ListAsync(Specification<T> spec, CancellationToken cancellationToken = default);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Specification<T> spec, CancellationToken cancellationToken = default);
        Task<T> FirstAsync(Specification<T> spec, CancellationToken cancellationToken = default);
        Task<T> FirstOrDefaultAsync(Specification<T> spec, CancellationToken cancellationToken = default);
    }
}
