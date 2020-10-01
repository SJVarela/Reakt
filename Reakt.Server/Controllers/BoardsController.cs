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
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly IBoardService _boardService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public BoardsController(IBoardService boardService, ILogger<BoardsController> logger, IMapper mapper)
        {
            _boardService = boardService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Board>> Get()
        {
            try
            {
                return Ok(_mapper.Map<IEnumerable<Board>>(_boardService.Get()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
