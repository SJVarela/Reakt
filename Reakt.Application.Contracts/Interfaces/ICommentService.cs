using Reakt.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Application.Contracts.Interfaces
{
    public interface ICommentService : ICrudServiceAsync<Comment>
    {
        Task<Comment> AddCommentAsync(long postId, Comment comment, CancellationToken? cancellationToken);

        Task<IEnumerable<Comment>> GetForPostAsync(long postId, int startRange, int endRange, string orderBy, CancellationToken? cancellationToken);

        Task<IEnumerable<Comment>> GetRepliesAsync(long parentId, int startRange, int endRange, CancellationToken? cancellationToken);

        void Like(long id);

        Task<Comment> ReplyAsync(long id, Comment comment, CancellationToken? cancellationToken);
    }
}