using MediatR;
using Reakt.Application.Contracts.Common;
using Reakt.Domain.Models;
using System.Collections.Generic;

namespace Reakt.Application.Posts.Queries
{
    public class GetPostsQuery : IRequest<IEnumerable<Post>>
    {
        public long BoardId { get; set; }
        public QueryFilter Filter { get; set; }
    }
}