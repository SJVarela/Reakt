using AutoMapper;
using Microsoft.AspNetCore.Http;
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
    [Route("api/posts/{postId}/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        public CommentsController(ILogger<CommentsController> logger, ICommentService commentService, IMapper mapper)
        {
            _logger = logger;
            _commentService = commentService;
            _mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>        
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Comment>>> GetForPost([FromRoute] long postId)
        {
            try
            {
                var result = await _commentService.GetForPost(postId);
                return Ok(_mapper.Map<IEnumerable<Comment>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpGet("{commentId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Comment>> Get(long commentId)
        {
            try
            {
                return Ok(_mapper.Map<Comment>(_commentService.Get(commentId)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="commentDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Comment>> AddComment(long postId, [FromBody] Comment commentDto)
        {
            try
            {
                var comment = _mapper.Map<Domain.Models.Comment>(commentDto);
                return Ok(_mapper.Map<Comment>(await _commentService.AddComment(postId, comment)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
