using MediatR;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Application.Posts.Commands.AddPost
{
    public class AddPostCommandHandler : IRequestHandler<AddPostCommand, Post>
    {
        private readonly IPostService _postService;

        public AddPostCommandHandler(IPostService postService)
        {
            _postService = postService;
        }

        public Task<Post> Handle(AddPostCommand request, CancellationToken cancellationToken)
        {
            return _postService.AddAsync(request.BoardId, request.Post, cancellationToken);
        }
    }
}