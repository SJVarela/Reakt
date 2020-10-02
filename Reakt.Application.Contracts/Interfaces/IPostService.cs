using Reakt.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reakt.Application.Contracts.Interfaces
{
    public interface IPostService : ICrudServiceAsync<Post>
    {
        Task<Post> AddAsync(long boardId, Post entity);
        Task<IEnumerable<Post>> GetForBoardAsync(long boardId);

    }
}
