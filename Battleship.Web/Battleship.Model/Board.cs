using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Model
{
    public class Board
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public BoardNode[][] BoardNodes = new BoardNode[10][];
    }
}
