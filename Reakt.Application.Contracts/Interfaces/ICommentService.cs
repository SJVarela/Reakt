using Reakt.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reakt.Application.Contracts.Interfaces
{
    public interface ICommentService : ICrudAsyncService<Comment>
    {
        Task<Comment> AddCommentAsync(long postId, Comment comment);

        Task<IEnumerable<Comment>> GetForPostAsync(long postId, int startRange, int endRange);

        void Like(long id);
    }
}