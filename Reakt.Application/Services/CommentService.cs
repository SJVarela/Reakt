using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Application.Persistence;
using Reakt.Domain.Models;
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
            throw new System.NotImplementedException();
        }

        public async Task DeleteAsync(long id)
        {
            _dbContext.Comments.Remove(_dbContext.Comments.First(x => x.Id == id));
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comment>> GetAsync()
        {
            return _mapper.Map<IEnumerable<Comment>>(await _dbContext.Comments.ToListAsync());
        }

        public async Task<Comment> GetAsync(long id)
        {
            var result = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<Comment>(result);
        }

        public async Task<IEnumerable<Comment>> GetForPostAsync(long postId, int startRange, int endRange)
        {
            var result = await _dbContext.Comments.Where(c => c.PostId == postId && c.ParentId == null)
                                         .OrderByDescending(c => c.CreatedAt)
                                         .Skip(startRange)
                                         .Take(endRange - startRange)
                                         .ToListAsync();
            return _mapper.Map<IEnumerable<Comment>>(result);
        }

        public void Like(long id)
        {
            throw new System.NotImplementedException();
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