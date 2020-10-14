using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Server.Controllers;
using Reakt.Server.MapperConfig;
using System.Linq;
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
        private BoardsController _boardController;
        private Fixture _fixture;
        private IMapper _mapper;

        [Test]
        public async Task GetAsyncById_Should_Return_OkResult()
        {
            //Arrange
            var id = 5L;
            var expected = _fixture.Build<DM.Board>()
                                   .With(x => x.Id, id)
                                   .Without(x => x.Posts)
                                   .Create();

            _boardService.Setup(s => s.GetAsync(id, null))
                           .ReturnsAsync(expected);

            //Act
            var result = (await _boardController.GetAsync(id)).Result as OkObjectResult;

            //Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(_mapper.Map<SM.Board>(expected));
        }

        [OneTimeSetUp]
        public void Setup()
        {
            _mapper = new Mapper(new MapperConfiguration(conf => conf.AddProfile(new BoardProfile())));
            _boardController = new BoardsController(_boardService.Object, _logger.Object, _mapper);
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                             .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());//recursionDepth
        }
    }
}