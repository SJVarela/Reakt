using MediatR;
using Reakt.Domain.Models;

namespace Reakt.Application.Comments.Queries
{
    public class GetCommentDetailQuery : IRequest<Comment>
    {
        public long Id { get; set; }
    }
}