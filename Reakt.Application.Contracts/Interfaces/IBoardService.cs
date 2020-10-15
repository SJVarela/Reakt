using Reakt.Application.Contracts.Common;
using Reakt.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Application.Contracts.Interfaces
{
    public interface IBoardService : ICrudServiceAsync<Board>
    {
        Task<IEnumerable<Board>> GetAsync(QueryFilter filter, CancellationToken? cancellationToken);
    }
}