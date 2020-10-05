using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Application.Persistence;
using Reakt.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<Comment> AddCommentAsync(long postId, Comment comment)
        {
            var storedComment = _mapper.Map<Persistence.Models.Comment>(comment);
            storedComment.PostId = postId;
            storedComment = _dbContext.Comments.Add(storedComment).Entity;
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<Comment>(storedComment);
        }

        public Task<Comment> CreateAsync(Comment entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Comment>> GetAsync()
        {
            return _mapper.Map<IEnumerable<Comment>>(
                await _dbContext.Comments.ToListAsync());
        }

        public async Task<Comment> GetAsync(long id)
        {
            var result = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<Comment>(result);
        }

        public async Task<IEnumerable<Comment>> GetForPostAsync(long postId, int startRange, int endRange)
        {
            return _mapper.Map<IEnumerable<Comment>>(
                await _dbContext.Comments.Where(c => c.PostId == postId)
                                         .OrderByDescending(c => c.CreatedAt)
                                         .Skip(startRange)
                                         .Take(endRange - startRange)
                                         .ToListAsync());
        }

        public void Like(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<Comment> UpdateAsync(Comment entity)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == entity.Id);
            _mapper.Map(entity, comment);

            var storedComment = _dbContext.Comments.Update(comment).Entity;
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<Comment>(storedComment);
        }
    }
}