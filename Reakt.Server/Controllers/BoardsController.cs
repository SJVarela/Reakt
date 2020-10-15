using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reakt.Application.Boards.Queries;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Server.Models;
using Reakt.Server.Models.Filters;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Server.Controllers
{
    /// <summary>
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly IBoardService _boardService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;


        /// <summary>
        /// </summary>
        /// <param name="boardService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public BoardsController(IBoardService boardService, ILogger<BoardsController> logger, IMapper mapper, IMediator mediator)
        {
            _boardService = boardService;
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
        }

        /// <summary>
        /// Get all boards
        /// </summary>
        /// <returns>A List of boards</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Board>>> GetAsync([FromQuery] QueryFilter filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var boards = await _mediator.Send(new GetBoardsQuery { 
                    Filter = _mapper.Map<Application.Contracts.Common.QueryFilter>(filter)
                });
                return Ok(_mapper.Map<IEnumerable<Board>>(boards));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get a board by id
        /// </summary>
        /// <returns>A board</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Board>>> GetAsync(long id)
        {
            try
            {
                var board = await _mediator.Send(new GetBoardDetailQuery { Id = id });
                if (board is null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<Board>(board));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}