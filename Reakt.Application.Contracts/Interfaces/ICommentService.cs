using Reakt.Application.Contracts.Common;
using Reakt.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Application.Contracts.Interfaces
{
    public interface ICommentService : ICrudServiceAsync<Comment>
    {
        Task<Comment> AddCommentAsync(long postId, Comment comment, CancellationToken? cancellationToken);

        Task<IEnumerable<Comment>> GetForPostAsync(long postId, QueryFilter filter, CancellationToken? cancellationToken);

        Task<IEnumerable<Comment>> GetRepliesAsync(long parentId, QueryFilter filter, CancellationToken? cancellationToken);

        void Like(long id);

        Task<Comment> ReplyAsync(long id, Comment comment, CancellationToken? cancellationToken);
    }
}