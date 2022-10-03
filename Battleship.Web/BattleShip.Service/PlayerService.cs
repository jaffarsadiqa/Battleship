using Battleship.Model;
using BattleShip.Service.Interface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using globalConst = Battleship.Common.Constants.Global;

namespace BattleShip.Service
{
    public class PlayerService : IPlayerService
    {
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<PlayerService> logger;
        public PlayerService(IMemoryCache memoryCache, ILogger<PlayerService> logger)
        {
            this.memoryCache = memoryCache;
            this.logger = logger;
        }

        public Player CreatePlayer(Player player)
        {   
            if(player == null)
            {
                throw new ArgumentNullException("Player object cannot be null");
            }
            player.Id = Guid.NewGuid();
            var key = string.Format("{0}-{1}", globalConst.PlayerCacheKey, player.Id);
            memoryCache.Set(key, player, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(globalConst.CacheExpiryInHours)));
            return player;
                 
        }

        public bool DeletePlayer(Guid idPlayer)
        {
            var key = string.Format("{0}-{1}", globalConst.PlayerCacheKey, idPlayer);
            memoryCache.Remove(key);
            throw new NotImplementedException();
        }

        public Player GetPlayer(Guid idPlayer)
        {
            var key = string.Format("{0}-{1}", globalConst.PlayerCacheKey, idPlayer);
            var memItem = memoryCache.Get(key);
            if(memItem == null)
            {
                throw new ArgumentNullException("Player id not found");
            }
            return (Player)memItem;
        }

        public Player UpdatePlayer(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException("Player object cannot be null");
            }
            var key = string.Format("{0}-{1}", globalConst.PlayerCacheKey, player.Id);
            memoryCache.Set(key, player, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(globalConst.CacheExpiryInHours)));
            return player;
        }
    }
}
