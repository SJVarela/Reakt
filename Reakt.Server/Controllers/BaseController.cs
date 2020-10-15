using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Reakt.Server.Controllers
{
    /// <summary>
    /// </summary>
    [Route("api")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        protected BaseController(IMediator mediator, IMapper mapper, ILogger logger)
        {
            Mapper = mapper;
            Logger = logger;
            Mediator = mediator;
        }

        /// <summary>
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// </summary>
        protected IMapper Mapper { get; }

        /// <summary>
        /// </summary>
        protected IMediator Mediator { get; }
    }
}