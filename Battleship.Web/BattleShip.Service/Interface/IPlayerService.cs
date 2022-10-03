using Battleship.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip.Service.Interface
{
    public interface IPlayerService
    {
        public Player CreatePlayer(Player player);
        public bool DeletePlayer(Guid idPlayer);
        public Player UpdatePlayer(Player player);
        public Player GetPlayer(Guid idPlayer);

    }
}
