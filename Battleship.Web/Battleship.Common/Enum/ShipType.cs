using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Common.Enum
{
    public enum ShipType
    {
        [Description("Aircraft Carrier")]
        Carrier = 1,
        [Description("Cruiser")]
        Cruiser = 2,
        [Description("Destroyer")]
        Destroyer = 3,
        [Description("Frigate")]
        Frigate = 4,
        [Description("Submarine")]
        Submarine = 5
    }
}
