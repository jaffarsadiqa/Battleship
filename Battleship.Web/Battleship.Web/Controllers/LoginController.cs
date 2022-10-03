using Azure.Core;
using Battleship.Common;
using Battleship.Common.Constants;
using Battleship.Common.ControllerBase;
using Battleship.Model;
using BattleShip.Service;
using BattleShip.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Battleship.Web.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBattleshipBase
    {
        private readonly ILogger<LoginController> logger;
        private readonly IPlayerService playerService;
        private readonly JwtConfig jwtConfig;
        public LoginController(ILogger<LoginController> logger, IPlayerService playerService,IOptions<JwtConfig> options)
        {
            this.logger = logger;
            this.playerService = playerService;
            this.jwtConfig = options.Value;
        }
    
        [HttpPost]
        public BaseResponse<string> RegisterPlayer(Player player)
        {
            var resp = new BaseResponse<string>();
            try
            {
                player = playerService.CreatePlayer(player);

                var issuer = jwtConfig.Issuer;
                var audience = jwtConfig.Audience;
                var key = Encoding.ASCII.GetBytes(jwtConfig.Key);

                var claims = new List<Claim>()
               .Append(new Claim(ClaimTypes.Name, player.Name))
               .Append(new Claim("Id", player.Id.ToString()));              

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(24),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                var stringToken = tokenHandler.WriteToken(token);
                resp.Result = stringToken;
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
