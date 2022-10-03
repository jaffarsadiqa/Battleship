using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Common.Constants
{
    public static class Global
    {
        public static string PlayerCacheKey = "player";
        public static string BoardCacheKey = "board";
        public static string ShipCacheKey = "ship";

        public static int CacheExpiryInHours = 24;
    }
}
