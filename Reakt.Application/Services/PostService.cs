using AutoMapper;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Application.Persistence;
using Reakt.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reakt.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IReaktDbContext _dbContext;
        private readonly IMapper _mapper;

        public PostService(IReaktDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Post Create(Post enity)
        {
            throw new NotImplementedException();
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Post> Get()
        {
            throw new NotImplementedException();
        }

        public Post Get(long id)
        {
            throw new NotImplementedException();
        }

        public Post Update(Post enity)
        {
            throw new NotImplementedException();
        }
    }
}
