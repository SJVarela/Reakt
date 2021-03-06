﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reakt.Application.Contracts.Common;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Application.Persistence;
using Reakt.Application.Persistence.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DM = Reakt.Domain.Models;

using PM = Reakt.Application.Persistence.Models;

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

        public async Task<DM.Comment> AddCommentAsync(long postId, DM.Comment comment, CancellationToken? cancellationToken)
        {
            var storedComment = _mapper.Map<PM.Comment>(comment);
            storedComment.PostId = postId;
            storedComment = _dbContext.Comments.Add(storedComment).Entity;
            await _dbContext.SaveChangesAsync(cancellationToken ?? new CancellationToken());
            return _mapper.Map<DM.Comment>(storedComment);
        }

        public Task<DM.Comment> CreateAsync(DM.Comment entity, CancellationToken? cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task DeleteAsync(long id, CancellationToken? cancellationToken)
        {
            _dbContext.Comments.Remove(_dbContext.Comments.First(x => x.Id == id));
            await _dbContext.SaveChangesAsync(cancellationToken ?? new CancellationToken());
        }

        public async Task<IEnumerable<DM.Comment>> GetAsync(CancellationToken? cancellationToken)
        {
            return _mapper.Map<IEnumerable<DM.Comment>>(await _dbContext.Comments.ToListAsync(cancellationToken ?? new CancellationToken()));
        }

        public async Task<DM.Comment> GetAsync(long id, CancellationToken? cancellationToken)
        {
            var result = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == id, cancellationToken ?? new CancellationToken());
            return _mapper.Map<DM.Comment>(result);
        }

        public async Task<IEnumerable<DM.Comment>> GetForPostAsync(long postId, QueryFilter filter, CancellationToken? cancellationToken)
        {
            var result = await _dbContext.Comments.Where(c => c.PostId == postId && c.ParentId == null)
                                                  .OrderByField(filter.OrderBy, filter.Ascending)
                                                  .Skip(filter.StartRange)
                                                  .Take(filter.EndRange - filter.StartRange)
                                                  .ToListAsync(cancellationToken ?? new CancellationToken());
            return _mapper.Map<IEnumerable<DM.Comment>>(result);
        }

        public async Task<IEnumerable<DM.Comment>> GetRepliesAsync(long parentId, QueryFilter filter, CancellationToken? cancellationToken)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == parentId);
            if (comment is null)
            {
                return null;
            }
            return _mapper.Map<IEnumerable<DM.Comment>>(
                await _dbContext.Comments.Include(c => c.Replies)
                                         .Where(c => c.Id == comment.Id)
                                         .SelectMany(c => c.Replies)
                                         .OrderByField(filter.OrderBy, filter.Ascending)
                                         .Skip(filter.StartRange)
                                         .Take(filter.EndRange - filter.StartRange)
                                         .ToListAsync(cancellationToken ?? new CancellationToken())
                                         );
        }

        public async Task<DM.Comment> LikeAsync(long id, CancellationToken? cancellationToken)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == id);
            comment.Likes++;
            await _dbContext.SaveChangesAsync(cancellationToken ?? new CancellationToken());

            return _mapper.Map<DM.Comment>(comment);
        }

        public async Task<DM.Comment> ReplyAsync(long id, DM.Comment comment, CancellationToken? cancellationToken)
        {
            var parentComment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (parentComment is null)
            {
                return null;
            }

            var storedComment = _mapper.Map<PM.Comment>(comment);
            storedComment.ParentId = parentComment.Id;
            storedComment.PostId = parentComment.PostId;

            storedComment = _dbContext.Comments.Add(storedComment).Entity;
            parentComment.ReplyCount++;

            await _dbContext.SaveChangesAsync(cancellationToken ?? new CancellationToken());
            return _mapper.Map<DM.Comment>(storedComment);
        }

        public async Task<DM.Comment> UpdateAsync(DM.Comment entity, CancellationToken? cancellationToken)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == entity.Id);
            _mapper.Map(entity, comment);
            await _dbContext.SaveChangesAsync(cancellationToken ?? new CancellationToken());
            return _mapper.Map<DM.Comment>(comment);
        }
    }
}