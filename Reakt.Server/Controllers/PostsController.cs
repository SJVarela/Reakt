using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reakt.Application.Posts.Commands.AddPost;
using Reakt.Application.Posts.Commands.DeletePost;
using Reakt.Application.Posts.Commands.UpdatePost;
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
    /// The Posts controller to handle requests for Posts api
    /// </summary>
    [Route("api")]
    [ApiController]
    public class PostsController : BaseController
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="logger">Injected logger</param>
        /// <param name="mapper">Injected mapper</param>
        /// <param name="mediator">Injected Mediator</param>
        public PostsController(ILogger<PostsController> logger, IMapper mapper, IMediator mediator) : base(mediator, mapper, logger)
        {
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
                var post = Mapper.Map<Domain.Models.Post>(postDto);
                return Ok(Mapper.Map<Post>(await Mediator.Send(new AddPostCommand { BoardId = boardId, Post = post })));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
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
                var post = await Mediator.Send(new GetPostDetailQuery { Id = id });
                if (post == null)
                {
                    return NotFound();
                }
                await Mediator.Send(new DeletePostCommand { Id = id });
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
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
                var post = await Mediator.Send(new GetPostDetailQuery { Id = id });
                if (post == null)
                {
                    return NotFound();
                }
                return Ok(Mapper.Map<Post>(post));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
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
                var result = await Mediator.Send(new GetPostsQuery
                {
                    BoardId = boardId,
                    Filter = Mapper.Map<Application.Contracts.Common.QueryFilter>(filter)
                });
                return Ok(Mapper.Map<IEnumerable<Post>>(result));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
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
                var post = Mapper.Map<Post>(await Mediator.Send(new GetPostDetailQuery { Id = id }));
                if (post == null)
                {
                    return NotFound();
                }
                patchDocument.ApplyTo(post, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var updatedPost = await Mediator.Send(new UpdatePostCommand { Post = Mapper.Map<DM.Post>(post) });
                return Ok(Mapper.Map<Post>(updatedPost));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}