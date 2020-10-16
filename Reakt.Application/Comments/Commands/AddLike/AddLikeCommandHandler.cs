using MediatR;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Application.Comments.Commands.AddLike
{
    public class AddLikeCommandHandler : IRequestHandler<AddLikeCommand, Comment>
    {
        private readonly ICommentService _commentService;

        public AddLikeCommandHandler(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public Task<Comment> Handle(AddLikeCommand request, CancellationToken cancellationToken)
        {
            return _commentService.LikeAsync(request.CommentId, cancellationToken);
        }
    }
}
