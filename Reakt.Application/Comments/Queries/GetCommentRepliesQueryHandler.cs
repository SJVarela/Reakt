using MediatR;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Application.Comments.Queries
{
    public class GetCommentRepliesQueryHandler : IRequestHandler<GetCommentRepliesQuery, IEnumerable<Comment>>
    {
        private readonly ICommentService _commentService;

        public GetCommentRepliesQueryHandler(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public Task<IEnumerable<Comment>> Handle(GetCommentRepliesQuery request, CancellationToken cancellationToken)
        {
            return _commentService.GetRepliesAsync(request.CommentId, request.StartRange, request.EndRange, cancellationToken);
        }
    }
}