﻿using AutoMapper;
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
        [HttpGet]
        public ActionResult<IEnumerable<Comment>> Get()
        {
            try
            {
                return Ok(_mapper.Map<IEnumerable<Comment>>(_commentService.Get()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }            
        }
        [HttpGet]
        [Route("{id}")]
        public Comment Get(long id)
        {
            _logger.LogInformation("I am informing");
            return _mapper.Map<Comment>(_commentService.Get(id));
        }
    }
}
