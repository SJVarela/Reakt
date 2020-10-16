using MediatR;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Application.Services;
using Reakt.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Application.Posts.Commands.DeletePost
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Post>
    {
        private readonly IPostService _postService;

        public DeletePostCommandHandler (IPostService postService)
        {
            _postService = postService;
        }

        public Task<Post> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            _postService.DeleteAsync(request.Id, cancellationToken);

            return null;
        }
    }
}
