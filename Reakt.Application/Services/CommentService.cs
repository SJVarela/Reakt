using Reakt.Application.Contracts.Interfaces;
using Reakt.Application.Persistence;
using Reakt.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Comment Get(long id)
        {
            var result = _dbContext.Comments.Where(x => x.Id == id).FirstOrDefault();
            return _mapper.Map<Comment>(result);
        }

        public IEnumerable<Comment> Get()
        {
            return _mapper.Map<IEnumerable<Comment>>(_dbContext.Comments.ToList());
        }

        public void Like(long id)
        {
            throw new NotImplementedException();
        }

        public Comment Update(Comment enity)
        {
            throw new NotImplementedException();
        }
    }
}
