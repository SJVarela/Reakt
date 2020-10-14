using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Application.Persistence;
using Reakt.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Application.Services
{
    public class BoardService : IBoardService
    {
        private readonly IReaktDbContext _dbContext;

        private readonly IMapper _mapper;

        public BoardService(IReaktDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task<Board> CreateAsync(Board entity, CancellationToken? cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(long id, CancellationToken? cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Board>> GetAsync(CancellationToken? cancellationToken)
        {
            return _mapper.Map<IEnumerable<Board>>(await _dbContext.Boards.ToListAsync(cancellationToken ?? new CancellationToken()));
        }

        public async Task<Board> GetAsync(long id, CancellationToken? cancellationToken)
        {
            return _mapper.Map<Board>(await _dbContext.Boards.FirstOrDefaultAsync(b => b.Id == id, cancellationToken ?? new CancellationToken()));
        }

        public Task<Board> UpdateAsync(Board entity, CancellationToken? cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}