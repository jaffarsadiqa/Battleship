using Battleship.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip.Service.Interface
{
    public interface IShipService
    {
        public Ship GetShip(Guid idShip);
        public Ship CreateShip(Ship ship);
        public bool DeleteShip(Guid idShip);
        public Ship UpdateShip(Ship ship);
    }
}
