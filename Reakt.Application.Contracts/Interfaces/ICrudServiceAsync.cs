using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reakt.Application.Contracts.Interfaces
{
    public interface ICrudServiceAsync<T>
    {
        Task<IEnumerable<T>> GetAsync();
        Task<T> GetAsync(long id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        void DeleteAsync(long id);
    }
}
