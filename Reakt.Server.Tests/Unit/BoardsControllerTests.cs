using AutoFixture;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Reakt.Application.Boards.Queries;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Server.Controllers;
using Reakt.Server.MapperConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DM = Reakt.Domain.Models;
using SM = Reakt.Server.Models;

namespace Reakt.Server.Tests.Unit
{
    [TestFixture]
    public class BoardsControllerTests
    {
        private readonly Mock<IBoardService> _boardService = new Mock<IBoardService>();
        private readonly Mock<ILogger<BoardsController>> _logger = new Mock<ILogger<BoardsController>>();
        private readonly Mock<IMediator> _mediator = new Mock<IMediator>();

        private BoardsController _boardsController;
        private Fixture _fixture;
        private IMapper _mapper;

        [Test]
        public async Task GetAsync_Error_Should_Return_ServerError_LogError()
        {
            //Arrange
            _mediator.Setup(x => x.Send(It.IsAny<GetBoardsQuery>(), It.IsAny<CancellationToken>()))
                        .Throws<Exception>();

            //Act
            var result = (await _boardsController.GetAsync(new SM.Filters.QueryFilter())).Result;

            //Assert
            result.Should().BeOfType<StatusCodeResult>();
            (result as StatusCodeResult).StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            _logger.Verify(x => x.Log(It.IsAny<LogLevel>(),
                                      It.IsAny<EventId>(),
                                      It.IsAny<It.IsAnyType>(),
                                      It.IsAny<Exception>(),
                                      (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }

        [Test]
        public async Task GetAsync_Should_Return_OkResult()
        {
            //Arrange
            var expected = _fixture.Build<DM.Board>().Without(x => x.Posts).CreateMany(2);

            _mediator.Setup(s => s.Send(It.IsAny<GetBoardsQuery>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(expected);

            //Act
            var result = (await _boardsController.GetAsync(new SM.Filters.QueryFilter())).Result as OkObjectResult;

            //Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(_mapper.Map<IEnumerable<SM.Board>>(expected));
        }

        [Test]
        public async Task GetAsyncById_Error_Should_Return_ServerError_LogError()
        {
            //Arrange
            _mediator.Setup(x => x.Send(It.IsAny<GetBoardDetailQuery>(), It.IsAny<CancellationToken>()))
                        .Throws<Exception>();

            //Act
            var result = (await _boardsController.GetAsync(1)).Result;

            //Assert
            result.Should().BeOfType<StatusCodeResult>();
            (result as StatusCodeResult).StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            _logger.Verify(x => x.Log(It.IsAny<LogLevel>(),
                                      It.IsAny<EventId>(),
                                      It.IsAny<It.IsAnyType>(),
                                      It.IsAny<Exception>(),
                                      (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }

        [Test]
        public async Task GetAsyncById_Should_Return_OkResult()
        {
            //Arrange
            var id = 5L;
            var expected = _fixture.Build<DM.Board>()
                                   .With(x => x.Id, id)
                                   .Without(x => x.Posts)
                                   .Create();

            _mediator.Setup(s => s.Send(It.IsAny<GetBoardDetailQuery>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(expected);

            //Act
            var result = (await _boardsController.GetAsync(id)).Result as OkObjectResult;

            //Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(_mapper.Map<SM.Board>(expected));
        }

        [Test]
        public async Task GetAsyncById_Wrong_Id_Should_Return_NotFoundResult()
        {
            //Arrange
            _mediator.Setup(s => s.Send(It.IsAny<GetBoardDetailQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((DM.Board)null);

            //Act
            var result = (await _boardsController.GetAsync(1)).Result;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [OneTimeSetUp]
        public void Setup()
        {
            _mapper = new Mapper(new MapperConfiguration(conf =>
            {
                conf.AddProfile(new BoardProfile());
                conf.AddProfile(new QueryFilterProfile());
            }));
            _boardsController = new BoardsController(_boardService.Object, _logger.Object, _mapper, _mediator.Object);
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                             .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());//recursionDepth
        }
    }
}