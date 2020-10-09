using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
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

        private readonly List<Post> _mockData = new List<Post>
        {
            new Post
            {
                Id = 1,
                Title = "Test post",
                Description = "A test description",
                CreatedAt = DateTime.Now,
                Comments = new List<Comment>()
            },
            new Post
            {
                Id = 2,
                Title = "Test 2 post",
                Description = "A test2 description",
                CreatedAt = DateTime.Now,
                Comments = new List<Comment>()
            }
        };

        private readonly Mock<IPostService> _postsService = new Mock<IPostService>();
        private IMapper _mapper;
        private PostsController _postsController;

        [Test]
        public async Task AddAsync_Error_Should_Return_ServerError_LogError()
        {
            //Arrange
            _postsService.Setup(x => x.AddAsync(It.IsAny<long>(), It.IsAny<Post>()))
                        .Throws<Exception>();

            //Act
            var result = (await _postsController.AddAsync(1, new Models.Post())).Result;

            //Assert
            result.Should().BeOfType<StatusCodeResult>();
            (result as StatusCodeResult).StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            _logger.Verify(x => x.Log(It.IsAny<LogLevel>(),
                                      It.IsAny<EventId>(),
                                      It.IsAny<It.IsAnyType>(),
                                      It.IsAny<Exception>(),
                                      (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }

        //TODO: AddAsync should return created item

        [Test]
        public async Task Get_Error_Should_Return_ServerError_LogError()
        {
            //Arrange
            _postsService.Setup(x => x.GetAsync())
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
            var expected = _mapper.Map<List<Server.Models.Post>>(_mockData);
            _postsService.Setup(s => s.GetAsync())
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
            _postsService.Setup(x => x.GetAsync(It.IsAny<long>()))
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
            _postsService.Setup(s => s.GetAsync(It.IsAny<long>()))
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
            _postsService.Setup(s => s.GetAsync(It.IsAny<long>()))
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
            var expected = _mapper.Map<List<Server.Models.Post>>(_mockData);
            _postsService.Setup(s => s.GetForBoardAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(_mockData);
            //Act
            var result = (await _postsController.GetForBoardAsync(1)).Result as OkObjectResult;

            //Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(expected);
        }

        [SetUp]
        public void Setup()
        {
            _mapper = new Mapper(new MapperConfiguration(conf => conf.AddProfile(new PostProfile())));
            _postsController = new PostsController(_postsService.Object, _logger.Object, _mapper);
        }

        [Test]
        public async Task UpdateAsync_Should_Return_UpdatedValues()
        {
            //Arrange
            var patchDocument = new JsonPatchDocument();
            patchDocument.Operations.Add(new Operation("add", "/description", "", "New post description"));
            var post = _mockData.First();
            patchDocument.ApplyTo(post);

            _postsService.Setup(x => x.UpdateAsync(It.IsAny<Post>())).ReturnsAsync(post);
            _postsService.Setup(x => x.GetAsync(It.IsAny<long>())).ReturnsAsync(post);

            //Act
            var result = (await _postsController.UpdateAsync(1, patchDocument)).Result as OkObjectResult;
            //Assert
            result.Value.Should().BeEquivalentTo(post);
        }
    }
}
