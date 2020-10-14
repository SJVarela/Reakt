using AutoMapper;
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
    [Route("api")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="mediator"></param>
        public CommentsController(IMediator mediator, ILogger<CommentsController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
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
                var comment = _mapper.Map<DM.Comment>(commentDto);
                return Ok(_mapper.Map<Comment>(await _mediator.Send(new AddCommentCommand { PostId = postId, Comment = comment })));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
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
                var comment = await _mediator.Send(new GetCommentDetailQuery { Id = id });
                return comment != null ? Ok(_mapper.Map<Comment>(comment)) : NotFound() as ActionResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets all the comments for a posts
        /// </summary>
        /// <param name="postId">Post identifier</param>
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
                var result = await _mediator.Send(new GetCommentsQuery
                {
                    PostId = postId,
                    Filter = _mapper.Map<Application.Contracts.Common.QueryFilter>(filter)
                });
                return Ok(_mapper.Map<IEnumerable<Comment>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
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
                var comment = await _mediator.Send(new GetCommentRepliesQuery
                {
                    CommentId = id,
                    Filter = _mapper.Map<Application.Contracts.Common.QueryFilter>(filter)
                });
                return comment != null ? Ok(_mapper.Map<IEnumerable<Comment>>(comment)) : NotFound() as ActionResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
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
                var comment = _mapper.Map<DM.Comment>(commentDto);
                var createdComment = await _mediator.Send(new AddReplyCommand { CommentId = id, Comment = comment });
                return createdComment != null ? Ok(_mapper.Map<Comment>(createdComment)) : NotFound() as ActionResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
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
                var comment = _mapper.Map<Comment>(await _mediator.Send(new GetCommentDetailQuery() { Id = id }));
                if (comment == null)
                {
                    return NotFound();
                }
                patchDocument.ApplyTo(comment, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var updatedComment = await _mediator.Send(new UpdateCommentCommand
                {
                    Comment = _mapper.Map<DM.Comment>(comment)
                });
                return Ok(_mapper.Map<Comment>(updatedComment));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}