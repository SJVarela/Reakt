using AutoFixture;
using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using Reakt.Application.Services;
using Reakt.Application.Tests.MockFactories;
using Reakt.Persistance.DataAccess;
using Reakt.Server.MapperConfig;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM = Reakt.Domain.Models;
using PM = Reakt.Application.Persistence.Models;

namespace Reakt.Application.Tests.Unit
{
    [TestFixture]
    internal class BoardServiceTests
    {
        private BoardService _boardService;

        private ReaktDbContext _context;

        private Fixture _fixture;
        private IMapper _mapper;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            _mapper = new Mapper(new MapperConfiguration(conf => conf.AddProfile(new BoardProfile())));
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                             .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());//recursionDepth
        }

        [Test]
        public async Task Get_by_Id_Should_Return_Results()
        {
            var id = _context.Boards.Max(x => x.Id);
            //Arrange
            var expected = _mapper.Map<DM.Board>(_context.Boards.First(b => b.Id == id));
            //Act
            var result = await _boardService.GetAsync(id);

            //Arrange
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task Get_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<List<DM.Board>>(_context.Boards.ToList());
            //Act
            var result = await _boardService.GetAsync();

            //Arrange
            result.Should().BeEquivalentTo(expected);
        }

        [SetUp]
        public void Setup()
        {
            _context = MockDbContextFactory.BuildInMemory(_mapper.Map<List<PM.Board>>(_fixture.Build<PM.Board>().Without(x => x.Posts).CreateMany(10)));
            _boardService = new BoardService(_context, _mapper);
        }
    }
}