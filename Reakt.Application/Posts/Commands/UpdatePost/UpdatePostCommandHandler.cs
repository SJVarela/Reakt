using MediatR;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Application.Posts.Commands.UpdatePost
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Post>
    {
        private readonly IPostService _postService;

        public UpdatePostCommandHandler(IPostService postService)
        {
            _postService = postService;
        }

        public Task<Post> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            return _postService.UpdateAsync(request.Post, cancellationToken);
        }
    }
}