using AutoMapper;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Application.Persistence;
using Reakt.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public Board Create(Board enity)
        {
            throw new NotImplementedException();
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Board> Get()
        {
            return _mapper.Map<IEnumerable<Board>>(_dbContext.Boards.ToList());
        }

        public Board Get(long id)
        {
            throw new NotImplementedException();
        }

        public Board Update(Board enity)
        {
            throw new NotImplementedException();
        }
    }
}
