using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Model.Extend
{
    public class ShipExtention:Ship
    {
        public bool IsDead
        {
            get
            {
                return HitBoardPoints != null && HitBoardPoints.Count >= BoardPointSize;
            }
        }
    }
}
