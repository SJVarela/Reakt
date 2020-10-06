using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Application.Persistence;
using Reakt.Domain.Models;
using System.Collections.Generic;
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

        public Task<Board> CreateAsync(Board entity)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(long id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Board>> GetAsync()
        {
            return _mapper.Map<IEnumerable<Board>>(await _dbContext.Boards.ToListAsync());
        }

        public async Task<Board> GetAsync(long id)
        {
            return _mapper.Map<Board>(await _dbContext.Boards.FirstOrDefaultAsync(b => b.Id == id));
        }

        public Task<Board> UpdateAsync(Board entity)
        {
            throw new System.NotImplementedException();
        }
    }
}