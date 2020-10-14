using MediatR;
using Reakt.Application.Contracts.Common;
using Reakt.Domain.Models;
using System.Collections.Generic;

namespace Reakt.Application.Comments.Queries
{
    public class GetCommentsQuery : IRequest<IEnumerable<Comment>>
    {
        public QueryFilter Filter { get; set; }
        public long PostId { get; set; }
    }
}