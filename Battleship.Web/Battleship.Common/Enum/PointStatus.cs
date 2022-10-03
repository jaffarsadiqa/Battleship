using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Common.Enum
{
    public enum PointStatus
    {
        [Description("Empty")]
        Empty =0,
        [Description("Occupied")]
        Occupied = 1
    }
}
