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
using System.Threading.Tasks;

namespace Reakt.Server.Controllers
{
    [Route("api")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public PostsController(IPostService postService, ILogger<BoardsController> logger, IMapper mapper)
        {
            _postService = postService;
            _logger = logger;
            _mapper = mapper;
        }

        [Route("posts")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Post>> Get()
        {
            try
            {
                return Ok(_mapper.Map<IEnumerable<Post>>(_postService.GetAsync()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("boards/{boardId}/posts")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Post>>> GetForBoard(long boardId)
        {
            try
            {
                return Ok(_mapper.Map<IEnumerable<Post>>(await _postService.GetForBoardAsync(boardId)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("boards/{boardId}/posts/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Post> GetById(long id)
        {
            try
            {
                return Ok(_mapper.Map<Post>(_postService.GetAsync(id)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("boards/{boardId}/posts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Post> Create(long boardId, [FromBody] Post postDto)
        {
            try
            {
                var post = _mapper.Map<Domain.Models.Post>(postDto);
                post.BoardId = boardId;
                return Ok(_mapper.Map<Post>(_postService.CreateAsync(post)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch("boards/{boardId}/posts")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Post> Update(JsonPatchDocument postDto)
        {
            try
            {
                // TODO: postDto.ApplyTo
                var post = _mapper.Map<Domain.Models.Post>(postDto);
                return Ok(_mapper.Map<Post>(_postService.UpdateAsync(post)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
