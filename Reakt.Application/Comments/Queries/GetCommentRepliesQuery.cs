using MediatR;
using Reakt.Application.Contracts.Common;
using Reakt.Domain.Models;
using System.Collections.Generic;

namespace Reakt.Application.Comments.Queries
{
    public class GetCommentRepliesQuery : IRequest<IEnumerable<Comment>>
    {
        public long CommentId { get; set; }
        public QueryFilter Filter { get; set; }
    }
}