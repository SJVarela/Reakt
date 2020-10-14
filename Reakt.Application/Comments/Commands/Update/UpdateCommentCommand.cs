using MediatR;
using Reakt.Domain.Models;

namespace Reakt.Application.Comments.Commands.Update
{
    public class UpdateCommentCommand : IRequest<Comment>
    {
        public Comment Comment { get; set; }
    }
}