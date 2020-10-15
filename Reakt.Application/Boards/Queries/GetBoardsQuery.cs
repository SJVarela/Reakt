using MediatR;
using Reakt.Application.Contracts.Common;
using Reakt.Domain.Models;
using System.Collections.Generic;

namespace Reakt.Application.Boards.Queries
{
    public class GetBoardsQuery : IRequest<IEnumerable<Board>>
    {
        public QueryFilter Filter { get; set; }
    }
}