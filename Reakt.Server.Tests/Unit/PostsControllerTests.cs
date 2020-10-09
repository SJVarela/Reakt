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
using DM = Reakt.Domain.Models;
using SM = Reakt.Server.Models;
using Reakt.Server.Controllers;
using Reakt.Server.MapperConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using System.Security.Cryptography;

namespace Reakt.Server.Tests.Unit
{
    [TestFixture]
    internal class PostsControllerTests
    {
        private readonly Mock<ILogger<PostsController>> _logger = new Mock<ILogger<PostsController>>();

        private readonly Mock<IPostService> _postsService = new Mock<IPostService>();
        private Fixture _fixture;
        private IMapper _mapper;
        private PostsController _postsController;

        [Test]
        public async Task AddAsync_Error_Should_Return_ServerError_LogError()
        {
            //Arrange
            _postsService.Setup(x => x.AddAsync(It.IsAny<long>(), It.IsAny<DM.Post>()))
                        .Throws<Exception>();

            //Act
            var result = (await _postsController.AddAsync(1, new SM.Post())).Result;

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
        public async Task AddAsync_Should_Return_Created_Item()
        {
            //Arrange
            var id = 20;
            var expected = _fixture.Build<DM.Post>()
                                    .With(x => x.Id, id)
                                    .Without(x => x.Comments)
                                    .Create();

            _postsService.Setup(x => x.AddAsync(It.IsAny<long>(), It.IsAny<DM.Post>()))
                        .ReturnsAsync(expected);

            //Act
            var result = (await _postsController.AddAsync(1, new SM.Post())).Result as OkObjectResult;

            //Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(_mapper.Map<SM.Post>(expected));
        }

        [Test]
        public async Task DeleteAsync_Error_Should_Return_ServerError_LogError()
        {
            //Arrange
            _postsService.Setup(x => x.GetAsync(It.IsAny<long>()))
                        .Throws<Exception>();
            _postsService.Setup(x => x.DeleteAsync(It.IsAny<long>()))
                        .Throws<Exception>();

            //Act
            var result = (await _postsController.DeleteAsync(1)).Result;

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
        public async Task DeleteAsync_Should_Execute_And_Return_Ok()
        {
            //Arrange
            var id = 20;
            var expected = _fixture.Build<DM.Post>()
                                    .With(x => x.Id, id)
                                    .Without(x => x.Comments)
                                    .Create();

            _postsService.Setup(x => x.GetAsync(It.IsAny<long>())).ReturnsAsync(expected);
            _postsService.Setup(x => x.DeleteAsync(It.IsAny<long>())).Verifiable();

            //Act
            var result = (await _postsController.DeleteAsync(1)).Result as OkResult;

            //Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            _postsService.Verify(m => m.DeleteAsync(It.IsAny<long>()), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_Should_Return_NotFound_When_Id_Not_Found()
        {
            //Arrange
            _postsService.Setup(x => x.GetAsync(It.IsAny<long>())).ReturnsAsync((DM.Post)null);

            //Act
            var result = (await _postsController.DeleteAsync(1)).Result;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

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
            var serviceResult = _fixture.Build<DM.Post>()
                                        .Without(x => x.Comments)
                                        .CreateMany(5);

            _postsService.Setup(s => s.GetAsync()).ReturnsAsync(serviceResult);

            //Act
            var result = (await _postsController.GetAsync()).Result as OkObjectResult;

            //Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(_mapper.Map<IEnumerable<SM.Post>>(serviceResult));
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
            var id = 5;
            var expected = _fixture.Build<DM.Post>()
                                   .With(x => x.Id, id)
                                   .Without(x => x.Comments)
                                   .Create();

            _postsService.Setup(s => s.GetAsync(It.IsAny<long>()))
                           .ReturnsAsync(expected);

            //Act
            var result = (await _postsController.GetByIdAsync(1)).Result as OkObjectResult;

            //Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(_mapper.Map<SM.Post>(expected));
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
            var expected = _fixture.Build<DM.Post>()
                                   .Without(x => x.Comments)
                                   .CreateMany();

            _postsService.Setup(s => s.GetForBoardAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(expected);
            //Act
            var result = (await _postsController.GetForBoardAsync(1)).Result as OkObjectResult;

            //Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(_mapper.Map<IEnumerable<SM.Post>>(expected));
        }

        [OneTimeSetUp]
        public void Setup()
        {
            _mapper = new Mapper(new MapperConfiguration(conf => conf.AddProfile(new PostProfile())));
            _postsController = new PostsController(_postsService.Object, _logger.Object, _mapper);
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                             .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());//recursionDepth
        }

        [Test]
        public async Task UpdateAsync_Should_Return_UpdatedValues()
        {
            //Arrange
            var patchDocument = new JsonPatchDocument();
            patchDocument.Operations.Add(new Operation("add", "/description", "", "New post description"));
            var post = _fixture.Build<DM.Post>()
                                   .Without(x => x.Comments)
                                   .Create();
            patchDocument.ApplyTo(post);

            _postsService.Setup(x => x.UpdateAsync(It.IsAny<DM.Post>())).ReturnsAsync(post);
            _postsService.Setup(x => x.GetAsync(It.IsAny<long>())).ReturnsAsync(post);

            //Act
            var result = (await _postsController.UpdateAsync(1, patchDocument)).Result as OkObjectResult;

            //Assert
            result.Value.Should().BeEquivalentTo(_mapper.Map<SM.Post>(post));
        }
    }
}