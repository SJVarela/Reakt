using MediatR;
using Reakt.Domain.Models;
using System.Collections.Generic;

namespace Reakt.Application.Comments.Queries
{
    public class GetCommentRepliesQuery : IRequest<IEnumerable<Comment>>
    {
        public long CommentId { get; set; }
        public int EndRange { get; set; }
        public int StartRange { get; set; }
    }
}