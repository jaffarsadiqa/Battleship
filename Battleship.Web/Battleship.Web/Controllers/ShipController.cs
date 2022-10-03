using Battleship.Common.ControllerBase;
using Battleship.Model;
using BattleShip.Service;
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
    public class ShipController : ControllerBattleshipBase
    {
        private readonly ILogger<ShipController> logger;
        private readonly IShipService shipService;
        public ShipController(ILogger<ShipController> logger, IShipService shipService)
        {
            this.logger = logger;
            this.shipService = shipService;
        }

        [HttpGet]
        public BaseResponse<Ship> Get(Guid idShip)
        {
            var resp = new BaseResponse<Ship>();
            try
            {
                resp.Result = shipService.GetShip(idShip);
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
        public BaseResponse<Ship> Create(Ship ship)
        {
            var resp = new BaseResponse<Ship>();
            try
            {
                resp.Result = shipService.CreateShip(ship);
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
        public BaseResponse<Ship> Update(Ship ship)
        {
            var resp = new BaseResponse<Ship>();
            try
            {
                resp.Result = shipService.UpdateShip(ship);
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
        public BaseResponse<bool> Delete(Guid idShip)
        {
            var resp = new BaseResponse<bool>();
            try
            {
                resp.Result = shipService.DeleteShip(idShip);
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
