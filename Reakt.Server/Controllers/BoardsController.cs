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
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly IBoardService _boardService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="boardService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public BoardsController(IBoardService boardService, ILogger<BoardsController> logger, IMapper mapper)
        {
            _boardService = boardService;
            _logger = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Board>> GetAsync()
        {
            try
            {
                return Ok(_mapper.Map<IEnumerable<Board>>(_boardService.GetAsync()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
