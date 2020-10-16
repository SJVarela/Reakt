using MediatR;
using Reakt.Domain.Models;

namespace Reakt.Application.Comments.Commands.AddLike
{
    public class AddLikeCommand : IRequest<Comment>
    {
        public long CommentId { get; set; }
    }
}