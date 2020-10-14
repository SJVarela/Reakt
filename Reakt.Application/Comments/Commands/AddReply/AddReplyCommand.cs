using MediatR;
using Reakt.Domain.Models;

namespace Reakt.Application.Comments.Commands.AddReply
{
    public class AddReplyCommand : IRequest<Comment>
    {
        public Comment Comment { get; set; }
        public long CommentId { get; set; }
    }
}