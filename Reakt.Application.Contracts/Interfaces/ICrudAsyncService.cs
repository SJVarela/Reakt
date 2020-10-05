using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reakt.Application.Contracts.Interfaces
{
    public interface ICrudAsyncService<T>
    {
        Task<T> CreateAsync(T enity);

        void Delete(long id);

        Task<IEnumerable<T>> GetAsync();

        Task<T> GetAsync(long id);

        Task<T> UpdateAsync(T enity);
    }
}