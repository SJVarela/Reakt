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
    public class PostService : IPostService
    {
        private readonly IReaktDbContext _dbContext;

        private readonly IMapper _mapper;

        public PostService(IReaktDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Create a Post in a Board
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="entity">Post properties</param>
        /// <returns>The created Post</returns>
        public async Task<Post> CreateAsync(long boardId, Post entity)
        {
            var post = _mapper.Map<Persistence.Models.Post>(entity);
            post.BoardId = boardId;
            var result = await _dbContext.Posts.AddAsync(post);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Post>(result);
        }

        public Task<Post> CreateAsync(Post entity)
        {
            throw new System.NotImplementedException();
        }

        public async void DeleteAsync(long id)
        {
            var post = _dbContext.Posts.First(x => x.Id == id);
            _dbContext.Posts.Remove(post);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Get All Posts
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Post>> GetAsync()
        {
            return _mapper.Map<IEnumerable<Post>>(await _dbContext.Posts.ToListAsync());
        }

        /// <summary>
        /// Get a Post by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Post with given id</returns>
        public async Task<Post> GetAsync(long id)
        {
            return _mapper.Map<Post>(await _dbContext.Posts.FirstOrDefaultAsync(x => x.Id == id));
        }

        /// <summary>
        /// Get all Posts for a given Board
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns>List of Posts in given Board</returns>
        public async Task<IEnumerable<Post>> GetForBoardAsync(long boardId)
        {
            return _mapper.Map<IEnumerable<Post>>(await _dbContext.Posts.Where(x => x.BoardId == boardId).ToListAsync());
        }

        /// <summary>
        /// Update a Post
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The updated Post</returns>
        public async Task<Post> UpdateAsync(Post entity)
        {
            var oldPost = await _dbContext.Posts.FindAsync(entity.Id);
            oldPost.Title = entity.Title;
            oldPost.Description = entity.Description;
            oldPost.BoardId = entity.BoardId;

            var newPost = _dbContext.Posts.Update(oldPost);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Post>(newPost);
        }
    }
}