using Reakt.Application.Contracts.Interfaces;
using Reakt.Application.Persistence;
using Reakt.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace Reakt.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly IReaktDbContext _dbContext;
        private readonly IMapper _mapper;

        public CommentService(IReaktDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public Comment Create(Comment enity)
        {
            throw new NotImplementedException();
        }

        public void Delete(ulong id)
        {
            throw new NotImplementedException();
        }

        public Comment Get(ulong id)
        {
            return _mapper.Map<Comment>(_dbContext.Comments.First(x => x.Id == id));
        }

        public IEnumerable<Comment> Get()
        {
            throw new NotImplementedException();
        }

        public void Like(ulong id)
        {
            throw new NotImplementedException();
        }

        public Comment Update(Comment enity)
        {
            throw new NotImplementedException();
        }
    }
}
