using MediatR;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Application.Comments.Queries
{
    public class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, IEnumerable<Comment>>
    {
        private readonly ICommentService _commentService;

        public GetCommentsQueryHandler(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public Task<IEnumerable<Comment>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
        {
            return _commentService.GetForPostAsync(request.PostId, request.Filter, cancellationToken);
        }
    }
}