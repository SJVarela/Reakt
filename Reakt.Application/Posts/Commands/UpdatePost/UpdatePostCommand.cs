using MediatR;
using Reakt.Domain.Models;

namespace Reakt.Application.Posts.Commands.UpdatePost
{
    public class UpdatePostCommand : IRequest<Post>
    {
        public Post Post { get; set; }
    }
}