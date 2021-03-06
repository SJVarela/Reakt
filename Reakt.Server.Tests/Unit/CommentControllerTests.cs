﻿using AutoFixture;
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
using Reakt.Application.Comments.Commands.AddLike;
using Reakt.Application.Comments.Commands.Update;
using Reakt.Application.Comments.Queries.GetCommentDetail;
using Reakt.Application.Comments.Queries.GetComments;
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
    public class CommentControllerTests
    {
        private readonly Mock<ILogger<CommentsController>> _logger = new Mock<ILogger<CommentsController>>();
        private readonly Mock<IMediator> _mediator = new Mock<IMediator>();
        private CommentsController _commentsController;
        private Fixture _fixture;
        private IMapper _mapper;

        [Test(Description = "AddLike on error should return ServerError and Log the error")]
        public async Task AddLikeAsync_OnError_Should_Return_ServerError_LogError()
        {
            //Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetCommentDetailQuery>(), It.IsAny<CancellationToken>())).Throws<Exception>();
            _mediator.Setup(m => m.Send(It.IsAny<AddLikeCommand>(), It.IsAny<CancellationToken>())).Throws<Exception>();

            //Act
            var result = (await _commentsController.AddLikeAsync(1)).Result;

            //Assert
            result.Should().BeOfType<StatusCodeResult>();
            (result as StatusCodeResult).StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            _logger.Verify(x => x.Log(It.IsAny<LogLevel>(),
                                      It.IsAny<EventId>(),
                                      It.IsAny<It.IsAnyType>(),
                                      It.IsAny<Exception>(),
                                      (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }

        [Test(Description = "AddLike should return Not Found when Comment is not found")]
        public async Task AddLikeAsync_Should_Return_NotFound_When_Comment_Not_Found()
        {
            //Arrange
            _mediator.Setup(x => x.Send(It.IsAny<GetCommentDetailQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((DM.Comment)null);

            //Act
            var result = (await _commentsController.AddLikeAsync(1)).Result;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
            (result as NotFoundResult).StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Test(Description = "AddLike should update values")]
        public async Task AddLikeAsync_Should_Return_UpdatedValues()
        {
            //Arrange
            var comment = _fixture.Create<DM.Comment>();
            var originalComment = _mapper.Map<DM.Comment>(comment);
            comment.Likes++;

            _mediator.Setup(x => x.Send(It.IsAny<GetCommentDetailQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(originalComment);
            _mediator.Setup(x => x.Send(It.IsAny<AddLikeCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(comment);
            //Act
            var result = (await _commentsController.AddLikeAsync(1)).Result as OkObjectResult;
            //Assert
            result.Value.Should().BeEquivalentTo(comment);
        }

        [Test]
        public async Task Get_by_Id_Error_Should_Return_ServerError_LogError()
        {
            //Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetCommentDetailQuery>(), It.IsAny<CancellationToken>())).Throws<Exception>();

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
            var id = 5;
            var expected = _fixture.Build<DM.Comment>()
                                   .With(x => x.Id, id)
                                   .Create();

            _mediator.Setup(s => s.Send(It.IsAny<GetCommentDetailQuery>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(expected);

            //Act
            var result = (await _commentsController.GetAsync(id)).Result as OkObjectResult;

            //Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(_mapper.Map<SM.Comment>(expected));
        }

        [Test]
        public async Task Get_by_Wrong_Id_Should_Return_NotFound()
        {
            //Arrange
            _mediator.Setup(s => s.Send(It.IsAny<GetCommentDetailQuery>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync((DM.Comment)null);

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
            var serviceResult = _fixture.CreateMany<DM.Comment>(10);

            _mediator.Setup(m => m.Send(It.IsAny<GetCommentsQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(serviceResult);
            //Act
            var result = (await _commentsController.GetForPostAsync(1, new SM.Filters.QueryFilter())).Result as OkObjectResult;

            //Assert
            result.Value.Should().BeEquivalentTo(_mapper.Map<IEnumerable<SM.Comment>>(serviceResult));
        }

        [OneTimeSetUp]
        public void Setup()
        {
            _mapper = new Mapper(new MapperConfiguration(conf =>
            {
                conf.AddProfile(new CommentProfile());
                conf.AddProfile(new QueryFilterProfile());
            }));
            _commentsController = new CommentsController(_mediator.Object, _mapper, _logger.Object);
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                             .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());//recursionDepth
        }

        [Test(Description = "Update should update values")]
        public async Task UpdateAsync_Should_Return_UpdatedValues()
        {
            //Arrange
            var patchDocument = new JsonPatchDocument<SM.Comment>();
            patchDocument.Operations.Add(new Operation<SM.Comment>("add", "/message", "", "New message"));
            var comment = _fixture.Create<SM.Comment>();
            var originalComment = _mapper.Map<SM.Comment>(comment);
            patchDocument.ApplyTo(comment);

            _mediator.Setup(x => x.Send(It.IsAny<GetCommentDetailQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mapper.Map<DM.Comment>(originalComment));
            _mediator.Setup(x => x.Send(It.IsAny<UpdateCommentCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mapper.Map<DM.Comment>(comment));
            //Act
            var result = (await _commentsController.UpdateAsync(1, patchDocument)).Result as OkObjectResult;
            //Assert
            result.Value.Should().BeEquivalentTo(comment);
        }
    }
}