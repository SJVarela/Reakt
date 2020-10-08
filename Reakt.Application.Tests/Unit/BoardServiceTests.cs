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

        private EntityFactory<DM.Board> _entityFactory = new EntityFactory<DM.Board>();
        private IMapper _mapper;

        [Test]
        public async Task Get_by_Id_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<DM.Board>(_context.Boards.First(b => b.Id == 1));
            //Act
            var result = await _boardService.GetAsync(1);

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
            _mapper = new Mapper(new MapperConfiguration(conf => conf.AddProfile(new BoardProfile())));
            _context = MockDbContextFactory.BuildInMemory(_mapper.Map<List<PM.Board>>(_entityFactory.BuildMockList(1, 4)));
            _boardService = new BoardService(_context, _mapper);
        }
    }
}