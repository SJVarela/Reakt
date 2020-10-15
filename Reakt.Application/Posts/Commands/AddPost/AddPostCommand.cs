using MediatR;
using Reakt.Domain.Models;

namespace Reakt.Application.Posts.Commands.AddPost
{
    public class AddPostCommand : IRequest<Post>
    {
        public long BoardId { get; set; }
        public Post Post { get; set; }
    }
}