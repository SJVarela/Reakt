using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Domain.Models;
using Reakt.Server.Controllers;
using Reakt.Server.MapperConfig;
using System;
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
        public async Task Get_by_Id_Error_Should_Return_ServerError_LogError()
        {
            //Arrange
            _commentService.Setup(s => s.GetAsync(It.IsAny<long>()))
                           .Throws<Exception>();

            //Act
            var result = (await _commentsController.GetAsync(1)).Result;

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
        public async Task Get_by_Id_Should_Return_OkResults()
        {
            //Arrange
            var id = 1;
            var expected = _mapper.Map<Server.Models.Comment>(_mockData.First(b => b.Id == id));
            _commentService.Setup(s => s.GetAsync(It.IsAny<long>()))
                           .ReturnsAsync((long c) => { return _mockData.First(x => x.Id == c); });

            //Act
            var result = (await _commentsController.GetAsync(id)).Result as OkObjectResult;

            //Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task Get_by_Wrong_Id_Should_Return_NotFound()
        {
            //Arrange
            _commentService.Setup(s => s.GetAsync(It.IsAny<long>()))
                           .ReturnsAsync((long c) => { return null; });

            //Act
            var result = (await _commentsController.GetAsync(1)).Result;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
            (result as NotFoundResult).StatusCode.Should().Be(StatusCodes.Status404NotFound);
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

            //Assert
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