using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Model
{
    public class BaseResponse<TResult>
    {
        public bool Success { get; set; } = true;
        public List<string> Message { get; set; } = new List<string>();
        public TResult? Result { get; set; }
    }
}
