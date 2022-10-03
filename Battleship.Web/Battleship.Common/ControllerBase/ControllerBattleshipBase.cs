using Battleship.Common.Constants;
using System.Security.Claims;

namespace Battleship.Common.ControllerBase
{
    public class ControllerBattleshipBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        public Guid Id
        {
            get
            {
                var claims = HttpContext.User.Claims;
                return new Guid(claims.First(c => c.Type == CustomClainTypes.Id).Value);
            }
        }
        public string Name
        {
            get
            {
                var claims = HttpContext.User.Claims;
                return claims.First(c => c.Type == ClaimTypes.Name).Value;
            }
        }
    }
}
