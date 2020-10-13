using MediatR;
using Reakt.Domain.Models;

namespace Reakt.Application.Comments.Commands.AddComment
{
    public class AddCommentCommand : IRequest<Comment>
    {
        public Comment Comment { get; set; }
        public long PostId { get; set; }
    }
}