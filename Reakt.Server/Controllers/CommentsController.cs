using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

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
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="commentService"></param>
        /// <param name="mapper"></param>
        public CommentsController(ILogger<CommentsController> logger, ICommentService commentService, IMapper mapper)
        {
            _logger = logger;
            _commentService = commentService;
            _mapper = mapper;
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
                var comment = await _commentService.GetAsync(id);
                if (comment == null)
                    return NotFound();
                return Ok(_mapper.Map<Comment>(comment));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //TODO: We should add pagination
        /// <summary>
        /// Gets all the comments for a posts
        /// </summary>        
        /// <param name="postId">Post identifier</param>
        /// <returns>List of comments</returns>
        [HttpGet("posts/{postId}/comments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Comment>>> GetForPostAsync([FromRoute] long postId)
        {
            try
            {
                var result = await _commentService.GetForPostAsync(postId);
                return Ok(_mapper.Map<IEnumerable<Comment>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
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
                var comment = _mapper.Map<Domain.Models.Comment>(commentDto);
                return Ok(_mapper.Map<Comment>(await _commentService.AddCommentAsync(postId, comment)));
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
                var comment = _mapper.Map<Comment>(await _commentService.GetAsync(id));
                if (comment == null)
                    return NotFound();
                patchDocument.ApplyTo(comment);
                var updatedComment = await _commentService.UpdateAsync(_mapper.Map<Domain.Models.Comment>(comment));
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
