using Battleship.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Model
{
    public class Ship
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int BoardPointSize { get; set; } = 1;
        public List<Coordinate>? HitBoardPoints { get; set; } = new List<Coordinate>();
        public List<Coordinate>? ShipPlacedNodes { get; set; } = new List<Coordinate>();
        public ShipType Type { get; set; }
    }
}
