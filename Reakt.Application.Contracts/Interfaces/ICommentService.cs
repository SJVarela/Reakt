using Reakt.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reakt.Application.Contracts.Interfaces
{
    public interface ICommentService : ICrudAsyncService<Comment>
    {
        void Like(long id);
        Task<IEnumerable<Comment>> GetForPostAsync(long postId, int startRange, int endRange);
        Task<Comment> AddCommentAsync(long postId, Comment comment);
    }
}
