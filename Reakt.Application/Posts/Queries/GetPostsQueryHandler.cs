using MediatR;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Application.Posts.Queries
{
    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, IEnumerable<Post>>
    {
        private readonly IPostService _postService;

        public GetPostsQueryHandler(IPostService postService)
        {
            _postService = postService;
        }

        public Task<IEnumerable<Post>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            return _postService.GetForBoardAsync(request.BoardId, request.Filter, cancellationToken);
        }
    }
}
