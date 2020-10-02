using Reakt.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reakt.Application.Contracts.Interfaces
{
    public interface ICommentService : ICrudService<Comment>
    {
        void Like(long id);
        Task<IEnumerable<Comment>> GetForPost(long postId);
        Task<Comment> AddComment(long postId, Comment comment);
    }
}
