using MediatR;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Application.Comments.Queries
{
    public class GetCommentDetailQueryHandler : IRequestHandler<GetCommentDetailQuery, Comment>
    {
        private readonly ICommentService _commentService;

        public GetCommentDetailQueryHandler(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public Task<Comment> Handle(GetCommentDetailQuery request, CancellationToken cancellationToken)
        {
            return _commentService.GetAsync(request.Id);
        }
    }
}