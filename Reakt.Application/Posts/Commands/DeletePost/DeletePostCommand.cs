using MediatR;
using Reakt.Domain.Models;

namespace Reakt.Application.Posts.Commands.DeletePost
{
    public class DeletePostCommand : IRequest<Post>
    {
        public long Id { get; set; }
    }
}