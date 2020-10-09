using Reakt.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reakt.Application.Contracts.Interfaces
{
    public interface ICommentService : ICrudServiceAsync<Comment>
    {
        Task<Comment> AddCommentAsync(long postId, Comment comment);

        Task<IEnumerable<Comment>> GetForPostAsync(long postId, int startRange, int endRange);

        Task<IEnumerable<Comment>> GetRepliesAsync(long parentId, int startRange, int endRange);

        void Like(long id);

        Task<Comment> ReplyAsync(long id, Comment comment);
    }
}