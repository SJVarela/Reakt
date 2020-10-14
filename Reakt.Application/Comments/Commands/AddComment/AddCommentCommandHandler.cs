using MediatR;
using Reakt.Application.Comments.Commands.AddComment;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Application.Comments.Commands
{
    public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Comment>
    {
        private readonly ICommentService _commentService;

        public AddCommentCommandHandler(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public Task<Comment> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            return _commentService.AddCommentAsync(request.PostId, request.Comment, cancellationToken);
        }
    }
}