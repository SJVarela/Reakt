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
    public class GetBoardDetailQueryHandler : IRequestHandler<GetBoardDetailQuery, Board>
    {
        private readonly IBoardService _boardService;

        public GetBoardDetailQueryHandler(IBoardService boardService)
        {
            _boardService = boardService;
        }

        public Task<Board> Handle(GetBoardDetailQuery request, CancellationToken cancellationToken)
        {
            return _boardService.GetAsync(request.Id, cancellationToken);
        }
    }
}
