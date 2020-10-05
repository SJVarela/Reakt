using Reakt.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reakt.Application.Contracts.Interfaces
{
    public interface IPostService : ICrudServiceAsync<Post>
    {
        Task<Post> CreateAsync(long boardId, Post entity);

        Task<IEnumerable<Post>> GetForBoardAsync(long boardId);
    }
}