using Battleship.Model;
using BattleShip.Service.Interface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using globalConst = Battleship.Common.Constants.Global;

namespace BattleShip.Service
{
    public class ShipService : IShipService
    {
        private readonly ILogger<ShipService> logger;
        private readonly IMemoryCache memoryCache;
        public ShipService(IMemoryCache memoryCache, ILogger<ShipService> logger)
        {
            this.memoryCache = memoryCache;
            this.logger = logger;
        }
        public Ship CreateShip(Ship ship)
        {
            if (ship == null)
            {
                throw new ArgumentNullException("Ship object cannot be null");
            }
            ship.Id = Guid.NewGuid();
            var key = string.Format("{0}-{1}", globalConst.ShipCacheKey, ship.Id);
            memoryCache.Set(key, ship, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(globalConst.CacheExpiryInHours)));
            return ship;
        }

        public bool DeleteShip(Guid idShip)
        {
            var key = string.Format("{0}-{1}", globalConst.ShipCacheKey, idShip);
            memoryCache.Remove(key);
            return true;
        }

        public Ship GetShip(Guid idShip)
        {
            var keyShip = string.Format("{0}-{1}", globalConst.ShipCacheKey, idShip);
            var memShip = memoryCache.Get(keyShip);
            if (memShip == null)
            {
                throw new Exception("Ship id not found");
            }
            return (Ship)memShip;
        }

        public Ship UpdateShip(Ship ship)
        {
            if (ship == null)
            {
                throw new ArgumentNullException("Board object cannot be null");
            }
            var key = string.Format("{0}-{1}", globalConst.ShipCacheKey, ship.Id);
            memoryCache.Set(key, ship);
            return ship;
        }
    }
}
