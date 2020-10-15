using MediatR;
using Reakt.Application.Comments.Commands.Update;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Application.Comments.Update
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, Comment>
    {
        private readonly ICommentService _commentService;

        public UpdateCommentCommandHandler(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public Task<Comment> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            return _commentService.UpdateAsync(request.Comment, cancellationToken);
        }
    }
}