using Battleship.Common.ControllerBase;
using Battleship.Model;
using BattleShip.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace Battleship.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBattleshipBase
    {
        private readonly ILogger<PlayerController> logger;
        private readonly IPlayerService playerService;
        public PlayerController(ILogger<PlayerController> logger,IPlayerService playerService)
        {
            this.logger = logger;
            this.playerService = playerService;
        }

        [HttpGet]
        public BaseResponse<Player> Get(Guid idPlayer)
        {
            var resp = new BaseResponse<Player>();
            try
            {
                resp.Result = playerService.GetPlayer(idPlayer);
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
        public BaseResponse<Player> Create(Player player)
        {
            var resp = new BaseResponse<Player>();
            try
            {
                resp.Result = playerService.CreatePlayer(player);
                resp.Success = true;
            }
            catch(Exception ex)
            {
                resp.Success=false;
                resp.Message.Add(ex.Message);
                logger.Log(LogLevel.Error,ex.Message);
            }
            return resp;
        }
    
        [HttpPut]
        public BaseResponse<Player> Update(Player player)
        {
            var resp = new BaseResponse<Player>();
            try
            {
                resp.Result = playerService.UpdatePlayer(player);
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
        public BaseResponse<bool> Delete(Guid idPlayer)
        {
            var resp = new BaseResponse<bool>();
            try
            {
                resp.Result = playerService.DeletePlayer(idPlayer);
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
