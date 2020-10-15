using Reakt.Application.Contracts.Common;
using Reakt.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Application.Contracts.Interfaces
{
    public interface IPostService : ICrudServiceAsync<Post>
    {
        Task<Post> AddAsync(long boardId, Post entity, CancellationToken? cancellationToken);

        Task<IEnumerable<Post>> GetForBoardAsync(long boardId, QueryFilter filter, CancellationToken? cancellationToken);
    }
}