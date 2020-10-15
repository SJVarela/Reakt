using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reakt.Application.Contracts.Common;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Application.Persistence;
using Reakt.Application.Persistence.Extensions;
using Reakt.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Application.Services
{
    /// <summary>
    /// The Post application service
    /// </summary>
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
        /// <param name="boardId">The Board where the Post belongs</param>
        /// <param name="entity">Post properties</param>
        /// <param name="cancellationToken">Async token for cancelling requests</param>
        /// <returns>The created Post</returns>
        public async Task<Post> AddAsync(long boardId, Post entity, CancellationToken? cancellationToken)
        {
            var post = _mapper.Map<Persistence.Models.Post>(entity);
            post.BoardId = boardId;
            var result = _dbContext.Posts.Add(post).Entity;
            await _dbContext.SaveChangesAsync(cancellationToken ?? new CancellationToken());

            return _mapper.Map<Post>(result);
        }

        /// <summary>
        /// Create a standalone Post
        /// </summary>
        /// <param name="entity">Post properties</param>
        /// <returns>Not implemented exception</returns>
        public Task<Post> CreateAsync(Post entity, CancellationToken? cancellationToken)
        {
            //No way to create a post without a board
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete a Post
        /// </summary>
        /// <param name="id">An existing Post identifier</param>
        public async Task DeleteAsync(long id, CancellationToken? cancellationToken)
        {
            var post = _dbContext.Posts.Find(id);
            _dbContext.Posts.Remove(post);
            await _dbContext.SaveChangesAsync(cancellationToken ?? new CancellationToken());
        }

        /// <summary>
        /// Get All Posts
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Post>> GetAsync(CancellationToken? cancellationToken)
        {
            return _mapper.Map<IEnumerable<Post>>(await _dbContext.Posts.ToListAsync( cancellationToken ?? new CancellationToken()));
        }

        /// <summary>
        /// Get a Post by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Post with given id</returns>
        public async Task<Post> GetAsync(long id, CancellationToken? cancellationToken)
        {
            return _mapper.Map<Post>(await _dbContext.Posts.FirstOrDefaultAsync(x => x.Id == id, cancellationToken ?? new CancellationToken()));
        }

        /// <summary>
        /// Get all Posts for a given Board
        /// </summary>
        /// <param name="boardId">Board identifier</param>
        /// <param name="filter">Filters for the query</param>
        /// <param name="cancellationToken">Async token for cancelling requests</param>
        /// <returns>List of Posts in given Board</returns>
        public async Task<IEnumerable<Post>> GetForBoardAsync(long boardId, QueryFilter filter, CancellationToken? cancellationToken)
        {
            var result = await _dbContext.Posts.Where(x => x.BoardId == boardId)
                                               .OrderByField(filter.OrderBy, filter.Ascending)
                                               .Skip(filter.StartRange)
                                               .Take(filter.EndRange - filter.StartRange)
                                               .ToListAsync(cancellationToken ?? new CancellationToken());

            return _mapper.Map<IEnumerable<Post>>(result);
        }

        /// <summary>
        /// Update a Post
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The updated Post</returns>
        public async Task<Post> UpdateAsync(Post entity, CancellationToken? cancellationToken)
        {
            var oldPost = await _dbContext.Posts.FindAsync(entity.Id);
            _mapper.Map(entity, oldPost);

            var newPost = _dbContext.Posts.Update(oldPost).Entity;
            await _dbContext.SaveChangesAsync(cancellationToken ?? new CancellationToken());

            return _mapper.Map<Post>(newPost);
        }
    }
}