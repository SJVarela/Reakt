using Reakt.Application.Contracts.Interfaces;
using Reakt.Application.Persistence;
using Reakt.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;

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

        public async Task<Comment> AddComment(long postId, Comment comment)
        {
            Persistence.Models.Comment storedComment = _mapper.Map<Persistence.Models.Comment>(comment);
            storedComment.PostId = postId;
            storedComment = _dbContext.Comments.Add(storedComment).Entity;
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<Comment>(storedComment);
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

        public async Task<IEnumerable<Comment>> Get()
        {            
            return _mapper.Map<IEnumerable<Comment>>(
                await _dbContext.Comments.ToListAsync());
        }

        public async Task<IEnumerable<Comment>> GetForPost(long postId)
        {
            return _mapper.Map<IEnumerable<Comment>>(
                await _dbContext.Comments.Where(c => c.PostId == postId)
                                         .OrderByDescending(c => c.CreatedAt)
                                         .ToListAsync());
        }

        public void Like(long id)
        {
            throw new NotImplementedException();
        }

        public Comment Update(Comment enity)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Comment> ICrudService<Comment>.Get()
        {
            throw new NotImplementedException();
        }
    }
}
