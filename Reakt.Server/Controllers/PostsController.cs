using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Application.Posts.Commands.AddPost;
using Reakt.Application.Posts.Queries;
using Reakt.Server.Models;
using Reakt.Server.Models.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM = Reakt.Domain.Models;

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
        private readonly IMediator _mediator;
        private readonly IPostService _postService;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="postService">Injected Post service</param>
        /// <param name="logger">Injected logger</param>
        /// <param name="mapper">Injected mapper</param>
        public PostsController(IPostService postService, ILogger<PostsController> logger, IMapper mapper, IMediator mediator)
        {
            _postService = postService;
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
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
                return Ok(_mapper.Map<Post>(await _mediator.Send(new AddPostCommand { BoardId = boardId, Post = post })));
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
                var post = await _postService.GetAsync(id, null);
                if (post == null)
                {
                    return NotFound();
                }
                await _postService.DeleteAsync(id, null);
                return Ok();
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Post>> GetByIdAsync(long id)
        {
            try
            {
                var post = await _mediator.Send(new GetPostDetailQuery { Id = id });
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
        /// <param name="filter">Filters</param>
        /// <returns>List of Posts</returns>
        [Route("boards/{boardId}/posts")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Post>>> GetForBoardAsync([FromRoute] long boardId, [FromRoute] QueryFilter filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _mediator.Send(new GetPostsQuery
                {
                    BoardId = boardId,
                    Filter = _mapper.Map<Application.Contracts.Common.QueryFilter>(filter)
                });
                return Ok(_mapper.Map<IEnumerable<Post>>(result));
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
        /// <param name="patchDocument">JsonPatchDocument specifying the changes to be applied to object</param>
        /// <returns>The updated post</returns>
        [HttpPatch("posts/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Post>> UpdateAsync(long id, [FromBody] JsonPatchDocument<Post> patchDocument)
        {
            try
            {
                var post = _mapper.Map<Post>(await _postService.GetAsync(id, null));
                if (post == null)
                {
                    return NotFound();
                }
                patchDocument.ApplyTo(post, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var updatedPost = await _postService.UpdateAsync(_mapper.Map<DM.Post>(post), null);
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