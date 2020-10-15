using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Application.Contracts.Interfaces
{
    public interface ICrudServiceAsync<T>
    {
        Task<T> CreateAsync(T entity, CancellationToken? cancellationToken);

        Task DeleteAsync(long id, CancellationToken? cancellationToken);

        Task<IEnumerable<T>> GetAsync(CancellationToken? cancellationToken);

        Task<T> GetAsync(long id, CancellationToken? cancellationToken);

        Task<T> UpdateAsync(T entity, CancellationToken? cancellationToken);
    }
}