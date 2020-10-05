using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reakt.Application.Contracts.Interfaces
{
    public interface ICrudServiceAsync<T>
    {
        Task<T> CreateAsync(T entity);

        void DeleteAsync(long id);

        Task<IEnumerable<T>> GetAsync();

        Task<T> GetAsync(long id);

        Task<T> UpdateAsync(T entity);
    }
}