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

namespace Reakt.Server.Tests.Unit
{
    [TestFixture]
    internal class PostsControllerTests
    {
        private readonly Mock<ILogger<PostsController>> _logger = new Mock<ILogger<PostsController>>();

        private readonly List<Post> _mockData = new List<Post>()
        {
            new Post()
            {
                Id = 1,
                Title = "Test post",
                Description = "A test description",
                BoardId = 1,
                CreatedAt = DateTime.Now
            },
            new Post()
            {
                Id = 2,
                Title = "Test 2 post",
                Description = "A test2 description",
                BoardId = 1,
                CreatedAt = DateTime.Now
            }
    };

        private readonly Mock<IPostService> _postService = new Mock<IPostService>();
        private IMapper _mapper;
        private PostsController _postsController;

        [Test]
        public async Task Get_Error_Should_Return_ServerError_LogError()
        {
            //Arrange
            _postService.Setup(x => x.GetAsync())
                        .Throws<Exception>();

            //Act
            var result = (await _postsController.GetAsync()).Result;

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
        public async Task Get_Should_Return_Ok_Result()
        {
            //Arrange
            var id = 1;
            var expected = _mapper.Map<List<Server.Models.Post>>(_mockData);
            _postService.Setup(s => s.GetAsync())
                           .ReturnsAsync(() => { return _mockData; });

            //Act
            var result = (await _postsController.GetAsync()).Result as OkObjectResult;

            //Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetById_Error_Should_Return_ServerError_LogError()
        {
            //Arrange
            _postService.Setup(x => x.GetAsync(It.IsAny<long>()))
                        .Throws<Exception>();

            //Act
            var result = (await _postsController.GetByIdAsync(1)).Result;

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
        public async Task GetById_Should_Return_Ok_Result()
        {
            //Arrange
            var id = 1;
            var expected = _mapper.Map<Server.Models.Post>(_mockData.First(b => b.Id == id));
            _postService.Setup(s => s.GetAsync(It.IsAny<long>()))
                           .ReturnsAsync((long c) => { return _mockData.First(x => x.Id == c); });

            //Act
            var result = (await _postsController.GetByIdAsync(1)).Result as OkObjectResult;

            //Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetById_Wrong_Id_Should_Return_NotFound()
        {
            //Arrange
            _postService.Setup(s => s.GetAsync(It.IsAny<long>()))
                           .ReturnsAsync((long c) => { return null; });

            //Act
            var result = (await _postsController.GetByIdAsync(123)).Result;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
            (result as NotFoundResult).StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Test]
        public async Task GetForBoard_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<List<Server.Models.Post>>(_mockData.Where(x => x.BoardId == 1));
            _postService.Setup(s => s.GetForBoardAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(_mockData.Where(x => x.BoardId == 1));
            //Act
            var result = (await _postsController.GetForBoardAsync(1)).Result as OkObjectResult;

            //Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(expected);
        }

        /// <summary>
        /// TODO: AddAsync, DeleteAsync, UpdateAsync
        /// </summary>

        [SetUp]
        public void Setup()
        {
            _mapper = new Mapper(new MapperConfiguration(conf => conf.AddProfile(new PostProfile())));
            _postsController = new PostsController(_postService.Object, _logger.Object, _mapper);
        }
    }
}