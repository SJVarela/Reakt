using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reakt.Application.Boards.Queries;
using Reakt.Server.Models;
using Reakt.Server.Models.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reakt.Server.Controllers
{
    /// <summary>
    /// </summary>
    [ApiController]
    public class BoardsController : BaseController
    {
        /// <summary>
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public BoardsController(IMediator mediator, IMapper mapper, ILogger<BoardsController> logger) : base(mediator, mapper, logger)
        {
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
                var boards = await Mediator.Send(new GetBoardsQuery
                {
                    Filter = Mapper.Map<Application.Contracts.Common.QueryFilter>(filter)
                });
                return Ok(Mapper.Map<IEnumerable<Board>>(boards));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
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
                var board = await Mediator.Send(new GetBoardDetailQuery { Id = id });
                if (board is null)
                {
                    return NotFound();
                }
                return Ok(Mapper.Map<Board>(board));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}