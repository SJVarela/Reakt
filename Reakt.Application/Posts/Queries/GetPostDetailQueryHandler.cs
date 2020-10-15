using MediatR;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Application.Posts.Queries
{
    public class GetPostDetailQueryHandler : IRequestHandler<GetPostDetailQuery, Post>
    {
        private readonly IPostService _postService;

        public GetPostDetailQueryHandler (IPostService postService)
        {
            _postService = postService;
        }

        public Task<Post> Handle(GetPostDetailQuery request, CancellationToken cancellationToken)
        {
            return _postService.GetAsync(request.Id, cancellationToken);
        }
    }
}