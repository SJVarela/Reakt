using AutoFixture;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Reakt.Application.Contracts.Common;
using Reakt.Application.Contracts.Interfaces;
using Reakt.Application.Posts.Commands.AddPost;
using Reakt.Application.Posts.Commands.DeletePost;
using Reakt.Application.Posts.Commands.UpdatePost;
using Reakt.Application.Posts.Queries;
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
    internal class PostsControllerTests
    {
        private readonly Mock<ILogger<PostsController>> _logger = new Mock<ILogger<PostsController>>();

        private readonly Mock<IMediator> _mediator = new Mock<IMediator>();
        private readonly Mock<IPostService> _postsService = new Mock<IPostService>();
        private Fixture _fixture;
        private IMapper _mapper;
        private PostsController _postsController;

        [Test]
        public async Task AddAsync_Error_Should_Return_ServerError_LogError()
        {
            //Arrange
            _mediator.Setup(x => x.Send(It.IsAny<AddPostCommand>(), It.IsAny<CancellationToken>()))
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

            _mediator.Setup(x => x.Send(It.IsAny<AddPostCommand>(), It.IsAny<CancellationToken>()))
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
            _mediator.Setup(x => x.Send(It.IsAny<GetPostDetailQuery>(), It.IsAny<CancellationToken>()))
                        .Throws<Exception>();
            _mediator.Setup(x => x.Send(It.IsAny<DeletePostCommand>(), It.IsAny<CancellationToken>()))
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

            _mediator.Setup(x => x.Send(It.IsAny<GetPostDetailQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(expected);
            _mediator.Setup(x => x.Send(It.IsAny<DeletePostCommand>(), It.IsAny<CancellationToken>())).Verifiable();

            //Act
            var result = (await _postsController.DeleteAsync(1)).Result as OkResult;

            //Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            _mediator.Verify(m => m.Send(It.IsAny<DeletePostCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_Should_Return_NotFound_When_Id_Not_Found()
        {
            //Arrange
            _mediator.Setup(x => x.Send(It.IsAny<GetPostDetailQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((DM.Post)null);

            //Act
            var result = (await _postsController.DeleteAsync(1)).Result;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }


        [Test]
        public async Task GetById_Error_Should_Return_ServerError_LogError()
        {
            //Arrange
            _mediator.Setup(x => x.Send(It.IsAny<GetPostDetailQuery>(), It.IsAny<CancellationToken>()))
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

            _mediator.Setup(s => s.Send(It.IsAny<GetPostDetailQuery>(), It.IsAny<CancellationToken>()))
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
            _mediator.Setup(s => s.Send(It.IsAny<GetPostDetailQuery>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync((DM.Post)null);

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
            var filter = new SM.Filters.QueryFilter { StartRange = 0, EndRange = 50, Ascending = true, OrderBy = "Id" };

            _mediator.Setup(s => s.Send(It.IsAny<GetPostsQuery>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(expected);
            //Act
            var result = (await _postsController.GetForBoardAsync(1, filter)).Result as OkObjectResult;

            //Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(_mapper.Map<IEnumerable<SM.Post>>(expected));
        }

        [OneTimeSetUp]
        public void Setup()
        {
            _mapper = new Mapper(new MapperConfiguration(conf =>
            {
                conf.AddProfile(new PostProfile());
                conf.AddProfile(new QueryFilterProfile());
            }));
            _postsController = new PostsController(_logger.Object, _mapper, _mediator.Object);
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                             .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());//recursionDepth
        }

        [Test]
        public async Task UpdateAsync_Should_Return_UpdatedValues()
        {
            //Arrange
            var patchDocument = new JsonPatchDocument<SM.Post>();
            patchDocument.Operations.Add(new Operation<SM.Post>("add", "/description", "", "New post description"));
            var post = _fixture.Build<SM.Post>().Create();
            patchDocument.ApplyTo(post);

            _mediator.Setup(x => x.Send(It.IsAny<GetPostDetailQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mapper.Map<DM.Post>(post));
            _mediator.Setup(x => x.Send(It.IsAny<UpdatePostCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mapper.Map<DM.Post>(post));

            //Act
            var result = (await _postsController.UpdateAsync(1, patchDocument)).Result as OkObjectResult;

            //Assert
            result.Value.Should().BeEquivalentTo(_mapper.Map<SM.Post>(post));
        }
    }
}