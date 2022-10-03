using Battleship.Common.ControllerBase;
using Battleship.Common.Enum;
using Battleship.Model;
using BattleShip.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Battleship.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BoardController : ControllerBattleshipBase
    {
        private readonly ILogger<BoardController> logger;
        private readonly IBoardService boardService;
        public BoardController(ILogger<BoardController> logger, IBoardService boardService)
        {
            this.logger = logger;
            this.boardService = boardService;
        }

        [HttpGet]
        public BaseResponse<Board> Get(Guid idBoard)
        {
            var resp = new BaseResponse<Board>();
            try
            {
                resp.Result = boardService.GetBoard(idBoard);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message.Add(ex.Message);
                logger.Log(LogLevel.Error, ex.Message);
            }
            return resp;
        }

        [HttpPost]
        public BaseResponse<Board> Create(Board board)
        {
            var resp = new BaseResponse<Board>();
            var idPlayer = base.Id;
            try
            {
                resp.Result = boardService.CreateBoard(board, idPlayer);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message.Add(ex.Message);
                logger.Log(LogLevel.Error, ex.Message);
            }
            return resp;
        }

        [HttpPut]
        public BaseResponse<Board> Update(Board board)
        {
            var resp = new BaseResponse<Board>();
            try
            {
                resp.Result = boardService.UpdateBoard(board);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message.Add(ex.Message);
                logger.Log(LogLevel.Error, ex.Message);
            }
            return resp;
        }

        [HttpDelete]
        public BaseResponse<bool> Delete(Guid idBoard)
        {
            var resp = new BaseResponse<bool>();
            try
            {
                resp.Result = boardService.DeleteBoard(idBoard);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message.Add(ex.Message);
                logger.Log(LogLevel.Error, ex.Message);
            }
            return resp;
        }

        [HttpPost("AssignShipToBoard")]
        public BaseResponse<bool> AssignShipToBoard(Guid idShip, Coordinate startCoordinate, ShipAlignType shipAlignType)
        {
            var resp = new BaseResponse<bool>();            
            try
            {
                var idPlayer = base.Id;
                var board = boardService.GetBoardForPlayer(idPlayer);

                resp.Result = boardService.AssignShipToBoard(board, idShip, startCoordinate, shipAlignType);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message.Add(ex.Message);
                logger.Log(LogLevel.Error, ex.Message);
            }
            return resp;
        }

        [HttpPost("AttackShip")]
        public BaseResponse<bool> AttackShip(Coordinate coordinate)
        {
            var resp = new BaseResponse<bool>();
            try
            {
                var idPlayer = base.Id;
                var board = boardService.GetBoardForPlayer(idPlayer);

                resp.Result = boardService.AttackShip(board, coordinate);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message.Add(ex.Message);
                logger.Log(LogLevel.Error, ex.Message);
            }
            return resp;
        }
    }
}
