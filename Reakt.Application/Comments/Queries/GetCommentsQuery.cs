using MediatR;
using Reakt.Domain.Models;
using System.Collections.Generic;

namespace Reakt.Application.Comments.Queries
{
    public class GetCommentsQuery : IRequest<IEnumerable<Comment>>
    {
        public int EndRange { get; set; }
        public string OrderBy { get; set; }
        public long PostId { get; set; }
        public int StartRange { get; set; }
    }
}