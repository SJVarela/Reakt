using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reakt.Application.Contracts.Interfaces
{
    public interface ICrudAsyncService<T>
    {
        Task<IEnumerable<T>> GetAsync();
        Task<T> GetAsync(long id);
        Task<T> CreateAsync(T enity);
        Task<T> UpdateAsync(T enity);
        void Delete(long id);
    }
}
