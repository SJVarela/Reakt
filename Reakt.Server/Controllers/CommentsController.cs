using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reakt.Application.Comments.Commands.AddComment;
using Reakt.Application.Comments.Commands.AddReply;
using Reakt.Application.Comments.Commands.Update;
using Reakt.Application.Comments.Queries.GetCommentDetail;
using Reakt.Application.Comments.Queries.GetComments;
using Reakt.Application.Comments.Queries.GetCommentsReplies;
using Reakt.Server.Models;
using Reakt.Server.Models.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM = Reakt.Domain.Models;

namespace Reakt.Server.Controllers
{
    /// <summary>
    /// Controller to handle requests for comments api
    /// </summary>
    [ApiController]
    public class CommentsController : BaseController
    {
        /// <summary>
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public CommentsController(IMediator mediator, IMapper mapper, ILogger<CommentsController> logger) : base(mediator, mapper, logger)
        {
        }

        /// <summary>
        /// Adds a comment to a post
        /// </summary>
        /// <param name="postId">Post identifier</param>
        /// <param name="commentDto">Comment model</param>
        /// <returns>The created comment</returns>
        [HttpPost("posts/{postId}/comments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Comment>> AddAsync(long postId, [FromBody] Comment commentDto)
        {
            try
            {
                var comment = Mapper.Map<DM.Comment>(commentDto);
                return Ok(Mapper.Map<Comment>(await Mediator.Send(new AddCommentCommand { PostId = postId, Comment = comment })));
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get a particular comment by its Id
        /// </summary>
        /// <param name="id">Comment unique identifier</param>
        /// <returns>Comment with requested Id</returns>
        [HttpGet("comments/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Comment>> GetAsync(long id)
        {
            try
            {
                var comment = await Mediator.Send(new GetCommentDetailQuery { Id = id });
                return comment != null ? Ok(Mapper.Map<Comment>(comment)) : NotFound() as ActionResult;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets all the comments for a posts
        /// </summary>
        /// <param name="postId">Post identifier</param>
        /// <param name="filter"></param>
        /// <returns>List of comments</returns>
        [HttpGet("posts/{postId}/comments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Comment>>> GetForPostAsync([FromRoute] long postId, [FromQuery] QueryFilter filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                throw new Exception();
                var result = await Mediator.Send(new GetCommentsQuery
                {
                    PostId = postId,
                    Filter = Mapper.Map<Application.Contracts.Common.QueryFilter>(filter)
                });
                return Ok(Mapper.Map<IEnumerable<Comment>>(result));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets a comment's replies
        /// </summary>
        /// <param name="id">Comment unique identifier</param>
        /// <returns>List of comment's replies</returns>
        [HttpGet("comments/{id}/replies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Comment>>> GetRepliesAsync(long id, QueryFilter filter)
        {
            try
            {
                var comment = await Mediator.Send(new GetCommentRepliesQuery
                {
                    CommentId = id,
                    Filter = Mapper.Map<Application.Contracts.Common.QueryFilter>(filter)
                });
                return comment != null ? Ok(Mapper.Map<IEnumerable<Comment>>(comment)) : NotFound() as ActionResult;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Adds a reply to a comment
        /// </summary>
        /// <param name="id">Comment identifier</param>
        /// <param name="commentDto">Comment model</param>
        /// <returns>The created comment</returns>
        [HttpPost("comments/{id}/replies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Comment>> ReplyAsync(long id, [FromBody] Comment commentDto)
        {
            try
            {
                var comment = Mapper.Map<DM.Comment>(commentDto);
                var createdComment = await Mediator.Send(new AddReplyCommand { CommentId = id, Comment = comment });
                return createdComment != null ? Ok(Mapper.Map<Comment>(createdComment)) : NotFound() as ActionResult;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Update a comment
        /// </summary>
        /// <param name="id">Comment identifier</param>
        /// <param name="patchDocument">Json patch document Comment model</param>
        /// <returns>The updated comment</returns>
        [HttpPatch("comments/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Comment>> UpdateAsync(long id, [FromBody] JsonPatchDocument<Comment> patchDocument)
        {
            try
            {
                var comment = Mapper.Map<Comment>(await Mediator.Send(new GetCommentDetailQuery() { Id = id }));
                if (comment == null)
                {
                    return NotFound();
                }
                patchDocument.ApplyTo(comment, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var updatedComment = await Mediator.Send(new UpdateCommentCommand
                {
                    Comment = Mapper.Map<DM.Comment>(comment)
                });
                return Ok(Mapper.Map<Comment>(updatedComment));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}