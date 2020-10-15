using MediatR;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Application.Boards.Queries
{
    public class GetBoardsQueryHandler : IRequestHandler<GetBoardsQuery, IEnumerable<Board>>
    {
        private readonly IBoardService _boardService;

        public GetBoardsQueryHandler(IBoardService boardService)
        {
            _boardService = boardService;
        }

        public Task<IEnumerable<Board>> Handle(GetBoardsQuery request, CancellationToken cancellationToken)
        {
            return _boardService.GetAsync(cancellationToken);
        }
    }
}
