using MediatR;
using Reakt.Domain.Models;

namespace Reakt.Application.Posts.Queries
{
    public class GetPostDetailQuery : IRequest<Post>
    {
        public long Id { get; set; }
    }
}