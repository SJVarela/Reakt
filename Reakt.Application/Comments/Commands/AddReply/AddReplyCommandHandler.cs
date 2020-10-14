using MediatR;
using Reakt.Application.Comments.Commands.AddReply;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Application.Comments.Commands
{
    public class AddReplyCommandHandler : IRequestHandler<AddReplyCommand, Comment>
    {
        private readonly ICommentService _commentService;

        public AddReplyCommandHandler(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public Task<Comment> Handle(AddReplyCommand request, CancellationToken cancellationToken)
        {
            return _commentService.ReplyAsync(request.CommentId, request.Comment, cancellationToken);
        }
    }
}