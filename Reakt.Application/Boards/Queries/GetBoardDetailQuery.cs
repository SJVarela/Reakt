using MediatR;
using Reakt.Domain.Models;

namespace Reakt.Application.Boards.Queries
{
    public class GetBoardDetailQuery : IRequest<Board>
    {
        public long Id { get; set; }
    }
}