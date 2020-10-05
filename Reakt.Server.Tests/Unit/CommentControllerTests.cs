using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Domain.Models;
using Reakt.Server.Controllers;
using Reakt.Server.MapperConfig;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reakt.Application.Tests.Unit
{
    [TestFixture]
    internal class CommentControllerTests
    {
        private readonly Mock<ICommentService> _commentService = new Mock<ICommentService>();

        private readonly Mock<ILogger<CommentsController>> _logger = new Mock<ILogger<CommentsController>>();

        private readonly List<Comment> _mockData = new List<Comment>()
        {
            new Comment()
            {
                Id = 1,
                Message = "Test message",
                Likes = 3
            }
        };

        private CommentsController _commentsController;
        private IMapper _mapper;

        [Test]
        public async Task Get_by_Id_Should_Return_Results()
        {
            //Arrange
            var id = 1;
            var expected = _mapper.Map<Server.Models.Comment>(_mockData.First(b => b.Id == id));
            _commentService.Setup(s => s.GetAsync(It.IsAny<long>()))
                           .ReturnsAsync((long c) => { return _mockData.First(x => x.Id == c); });

            //Act
            var result = (await _commentsController.GetAsync(id)).Result as OkObjectResult;

            //Arrange
            result.Value.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task Get_For_Post_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<List<Server.Models.Comment>>(_mockData);
            _commentService.Setup(s => s.GetForPostAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(_mockData);
            //Act
            var result = (await _commentsController.GetForPostAsync(1)).Result as OkObjectResult;

            //Arrange
            result.Value.Should().BeEquivalentTo(expected);
        }

        [OneTimeSetUp]
        public void Setup()
        {
            _mapper = new Mapper(new MapperConfiguration(conf => conf.AddProfile(new CommentProfile())));
            _commentsController = new CommentsController(_logger.Object, _commentService.Object, _mapper);
        }
    }
}