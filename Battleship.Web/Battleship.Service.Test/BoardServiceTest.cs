using Battleship.Model;
using BattleShip.Service;
using BattleShip.Service.Interface;
using Castle.Components.DictionaryAdapter;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Policy;

namespace Battleship.Service.Test
{
    public class BoardServiceTest
    {
        private readonly ILogger<BoardService> logger;
        private readonly IMemoryCache memoryCache;
        private readonly IShipService shipService;
        

        //Sample global object
        Board board;
        Ship ship;
        public BoardServiceTest()
        {
            this.logger = new Mock<ILogger<BoardService>>().Object;
            this.memoryCache = new Mock<IMemoryCache>().Object;
            this.shipService = new Mock<IShipService>().Object;
        }
        [SetUp]
        public void Setup()
        {
            board = new Board() { Name = "Test User" };
            board.Id = Guid.NewGuid();
            board.BoardNodes = new BoardNode[10][];
            for (int i = 0; i < 10; i++)
            {
                board.BoardNodes[i] = new BoardNode[10];
                for (int j = 0; j < 10; j++)
                {
                    board.BoardNodes[i][j] = new BoardNode();
                }
            }

            ship = new Ship()
            {
                Id = new Guid("d005e562-b900-4936-804e-2e38f95c78c3"),
                Name = "INS Destroyer",
                Type = Common.Enum.ShipType.Destroyer,
                BoardPointSize = 3,
            };
        }

        [Test]
        public void CreateBoardThrowNullException()
        {
            var boardService = new BoardService(memoryCache, shipService, logger);
            Assert.Throws<ArgumentNullException>(() => boardService.CreateBoard(null,Guid.NewGuid()));
        }
        [Test]
        public void CreateBoard()
        {
            var boardService = new BoardService(memoryCache, shipService, logger);

            var memCache = Mock.Of<IMemoryCache>();
            var cachEntry = Mock.Of<ICacheEntry>();
            var mockMemoryCache = Mock.Get(memoryCache);
            mockMemoryCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(cachEntry);
            var cachedResponse = memoryCache.Set<string>(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>());

            var res = boardService.CreateBoard(new Board() { Name = "Test User" }, Guid.NewGuid());
            Assert.IsTrue(res!=null);
        }

        [Test]
        public void AssignShipToBoardThrowNullException()
        {
            var boardService = new BoardService(memoryCache, shipService, logger);
            var idShip = new Guid("d005e562-b900-4936-804e-2e38f95c78c3");

            Assert.Throws<ArgumentNullException>(() => boardService.AssignShipToBoard(board,idShip,null,Common.Enum.ShipAlignType.Horizontal));
        }

        [Test]
        public void AssignShipToBoardThrowCoordinateOutofBoardException()
        {
            var boardService = new BoardService(memoryCache, shipService, logger);
            var idShip = new Guid("d005e562-b900-4936-804e-2e38f95c78c3");

            Assert.Throws<Exception>(() => boardService.AssignShipToBoard(board, idShip, new Coordinate() { Row=8,Column=11}, Common.Enum.ShipAlignType.Horizontal));
            Assert.Throws<Exception>(() => boardService.AssignShipToBoard(board, idShip, new Coordinate() { Row = 11, Column = 9 }, Common.Enum.ShipAlignType.Horizontal));
            Assert.Throws<Exception>(() => boardService.AssignShipToBoard(board, idShip, new Coordinate() { Row = 8, Column = -1 }, Common.Enum.ShipAlignType.Horizontal));
            Assert.Throws<Exception>(() => boardService.AssignShipToBoard(board, idShip, new Coordinate() { Row = -1, Column = 7 }, Common.Enum.ShipAlignType.Horizontal));
        }

        // Due to lack of time, not planning to write rest of the test case.
        // Need to cover both postive and negative test cases to have an efficient unit test
    }
}