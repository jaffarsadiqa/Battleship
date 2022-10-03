using Battleship.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Model
{
    public class BoardNode
    {        
        public HitType HitType { get; set; } = HitType.Empty;
        public Ship Ship { get; set; }
    }
}
