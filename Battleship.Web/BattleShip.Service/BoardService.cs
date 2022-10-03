using Battleship.Common.Enum;
using Battleship.Model;
using BattleShip.Service.Interface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using globalConst = Battleship.Common.Constants.Global;

namespace BattleShip.Service
{
    public class BoardService : IBoardService
    {
        private readonly IMemoryCache memoryCache;
        private readonly IShipService shipService;
        private readonly ILogger<BoardService> logger;
        public BoardService(IMemoryCache memoryCache,IShipService shipService, ILogger<BoardService> logger)
        {
            this.memoryCache = memoryCache;
            this.shipService = shipService;
            this.logger = logger;
        }

        public bool AssignShipToBoard(Board board, Guid idShip, Coordinate startCoordinate, ShipAlignType shipAlignType)
        {
            if (startCoordinate == null)
            {
                throw new ArgumentNullException("Startcoordinate cannot be null");
            }

            //Make cordinate 0 based start
            startCoordinate = new Coordinate() { Row = startCoordinate.Row - 1, Column = startCoordinate.Column - 1 };

            if(startCoordinate.Row < 0 || startCoordinate.Row>9 || startCoordinate.Column<0 || startCoordinate.Column > 9)
            {
                throw new Exception("Startcoordinate range should be between Row - 1 to 10 and Column - 1 to 10");
            }        
                   
            Ship ship = shipService.GetShip(idShip);

            //if already ship assigned to a node, unassign those nodes from ship
            UnassignShipAlreadyPlacedInBoard(board, ship);
            

            //Validate the coordinate
            List<Coordinate> coords = new List<Coordinate>();

            //var isValid = true;
            var currCoOrd = startCoordinate;
            for (int i = 0; i < ship.BoardPointSize; i++)
            {
                if (ValidateNodeOccupiedAndAddCoordinate(currCoOrd, board, shipAlignType, ship))
                {
                    coords.Add(currCoOrd);
                }
                else
                {
                    throw new Exception("Coordinate is not empty to be placed");
                }
                currCoOrd = shipAlignType == ShipAlignType.Horizontal ? new Coordinate() { Row = currCoOrd.Row, Column = currCoOrd.Column + 1 } : new Coordinate() { Row = currCoOrd.Row + 1, Column = currCoOrd.Column };
            }
            UpdateBoard(board);
            shipService.UpdateShip(ship);
            return true;
        }

        public bool AttackShip(Board board, Coordinate coordinate)
        {
            if (coordinate == null)
            {
                throw new ArgumentNullException("Coordinate cannot be null");
            }

            //Make cordinate 0 based start
            coordinate = new Coordinate() { Row = coordinate.Row - 1, Column = coordinate.Column - 1 };

            if (coordinate.Row < 0 || coordinate.Row > 9 || coordinate.Column < 0 || coordinate.Column > 9)
            {
                throw new Exception("coordinate range should be between Row - 1 to 10 and Column - 1 to 10");
            }

            var boardNode = board.BoardNodes[coordinate.Row][coordinate.Column];
            if (boardNode.HitType == HitType.Empty)
            {
                boardNode.HitType = boardNode.Ship != null ?  HitType.PinHit : HitType.PinMiss;
                if (boardNode.Ship != null)
                {                    
                    Ship ship = shipService.GetShip(boardNode.Ship.Id);
                    ship.HitBoardPoints.Add(coordinate);
                    shipService.UpdateShip(ship);
                }
                UpdateBoard(board);
            }

            return boardNode.HitType==HitType.PinHit?true:false;
        }

        public Board CreateBoard(Board board,Guid idPlayer)
        {
            if(board == null)
            {
                throw new ArgumentNullException("Board object cannot be null");
            }            
            board.Id = Guid.NewGuid();
            board.BoardNodes = new BoardNode[10][];
            for(int i = 0; i < 10; i++)
            {
                board.BoardNodes[i] = new BoardNode[10];
                for (int j=0;j< 10; j++)
                {
                    board.BoardNodes[i][j] = new BoardNode();
                }
            }
            var key = string.Format("{0}-{1}", globalConst.BoardCacheKey, board.Id);
            memoryCache.Set(key, board, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(globalConst.CacheExpiryInHours)));

            var PlyBrdKey = string.Format("{0}-{1}-{2}", globalConst.PlayerCacheKey,idPlayer, globalConst.BoardCacheKey, idPlayer);
            memoryCache.Set(PlyBrdKey, board.Id, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(globalConst.CacheExpiryInHours)));
            
            return board;
        }

        public bool DeleteBoard(Guid idBoard)
        {
            var key = string.Format("{0}-{1}", globalConst.BoardCacheKey, idBoard);
            memoryCache.Remove(key);
            return true;
        }

        public Board GetBoard(Guid idBoard)
        {
            var key = string.Format("{0}-{1}", globalConst.BoardCacheKey, idBoard);
            var memItem = memoryCache.Get(key);

            if (memItem == null)
            {
                throw new Exception("Board id not found");
            }
            return (Board)memItem;
        }

        public Board GetBoardForPlayer(Guid idPlayer)
        {
            var PlyBrdKey = string.Format("{0}-{1}-{2}", globalConst.PlayerCacheKey, idPlayer, globalConst.BoardCacheKey, idPlayer);

            var memItem=memoryCache.Get(PlyBrdKey);
            if (memItem == null)
            {
                throw new Exception("Board id not found for this player");
            }            
            return GetBoard((Guid)memItem);
        }

        public Board UpdateBoard(Board board)
        {
            if (board == null)
            {
                throw new ArgumentNullException("Board object cannot be null");
            }
            var key = string.Format("{0}-{1}", globalConst.BoardCacheKey, board.Id);
            memoryCache.Set(key, board);
            return board;
        }

        private bool ValidateNodeOccupiedAndAddCoordinate(Coordinate currCoOrd, Board board, ShipAlignType shipAlignType,Ship ship)
        {            
            if (currCoOrd.Row < 0 || currCoOrd.Row > 9 || currCoOrd.Column < 0 || currCoOrd.Column > 9)
            {
                throw new Exception("Ship size and start coordinates leading outside the range");
            }
            var boardNode = board.BoardNodes[currCoOrd.Row][currCoOrd.Column];
            if (boardNode.HitType != HitType.Empty)
            {
                //Node is already been hit or marked
                return false;
            }
            if (boardNode.Ship != null)
            {
                //Already another ship occupied
                return false;
            }
            boardNode.Ship=ship;
            ship.ShipPlacedNodes.Add(currCoOrd);
            return true;
        }

        private void UnassignShipAlreadyPlacedInBoard(Board board,Ship ship)
        {
            if (ship.ShipPlacedNodes.Any())
            {
                foreach (var node in ship.ShipPlacedNodes)
                {
                    board.BoardNodes[node.Row][node.Column].Ship = null;
                }
                ship.ShipPlacedNodes.Clear();
            }
            
        }
    }    
}
