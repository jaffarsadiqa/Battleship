using Battleship.Common.Enum;
using Battleship.Model;

namespace BattleShip.Service.Interface
{
    public interface IBoardService
    {
        public Board CreateBoard(Board board, Guid idPlayer);
        public Board GetBoard(Guid idBoard);
        public bool DeleteBoard(Guid idBoard);
        public Board UpdateBoard(Board board);
        public bool AssignShipToBoard(Board board,Guid idShip, Coordinate startCoordinate, ShipAlignType shipAlignType);
        public bool AttackShip(Board board, Coordinate coordinate);
        public Board GetBoardForPlayer(Guid idPlayer);
    }
}