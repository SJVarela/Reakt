using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Server.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reakt.Server.Controllers
{
    /// <summary>
    /// The Posts controller
    /// </summary>
    [Route("api")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IPostService _postService;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="postService">Injected Post service</param>
        /// <param name="logger">Injected logger</param>
        /// <param name="mapper">Injected mapper</param>
        public PostsController(IPostService postService, ILogger<PostsController> logger, IMapper mapper)
        {
            _postService = postService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Add a Post to a Board
        /// </summary>
        /// <param name="boardId">Board identfier</param>
        /// <param name="postDto">Post model</param>
        /// <returns>The created Post</returns>
        [HttpPost("boards/{boardId}/posts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Post>> AddAsync(long boardId, [FromBody] Post postDto)
        {
            try
            {
                var post = _mapper.Map<Domain.Models.Post>(postDto);
                return Ok(_mapper.Map<Post>(await _postService.AddAsync(boardId, post)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delete a Post
        /// </summary>
        /// <param name="id">Post identifier</param>
        /// <returns>Ok if Post was deleted</returns>
        [HttpDelete("posts/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Post>> DeleteAsync(long id)
        {
            try
            {
                var post = await _postService.GetAsync(id);
                if (post == null)
                {
                    return NotFound();
                }
                await _postService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get all Posts
        /// </summary>
        /// <returns>List of Posts</returns>
        [Route("posts")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Post>>> Get()
        {
            try
            {
                var posts = await _postService.GetAsync();
                return Ok(_mapper.Map<IEnumerable<Post>>(posts));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets a Post by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("posts/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Post>> GetById(long id)
        {
            try
            {
                var post = await _postService.GetAsync(id);
                if (post == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<Post>(post));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get all Posts for a Board
        /// </summary>
        /// <param name="boardId">Board identifier</param>
        /// <param name="startRange">Number of the item to start at</param>
        /// <param name="endRange">Number of the item to finish at</param>
        /// <returns>List of Posts</returns>
        [Route("boards/{boardId}/posts")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Post>>> GetForBoardAsync([FromRoute] long boardId, int startRange = 0, int endRange = 50)
        {
            if (startRange > endRange)
            {
                return BadRequest("Pagination values are not valid");
            }

            try
            {
                var posts = await _postService.GetForBoardAsync(boardId, startRange, endRange);
                if (posts == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<IEnumerable<Post>>(posts));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Update a Post
        /// </summary>
        /// <param name="id">Post identifier</param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch("posts/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Post>> UpdateAsync(long id, [FromBody] JsonPatchDocument patchDocument)
        {
            try
            {
                var post = await _postService.GetAsync(id);
                if (post == null)
                {
                    return NotFound();
                }
                patchDocument.ApplyTo(post);
                var updatedPost = await _postService.UpdateAsync(post);
                return Ok(_mapper.Map<Post>(updatedPost));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}