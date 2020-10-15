using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Reakt.Server.Controllers
{
    [Route("api")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected ILogger Logger { get; }
        protected IMapper Mapper { get; }
        protected IMediator Mediator { get; }

        public BaseController(IMediator mediator, IMapper mapper, ILogger logger)
        {
            Mapper = mapper;
            Logger = logger;
            Mediator = mediator;
        }
    }
}