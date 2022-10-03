using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Battleship.Model.Validators
{
    public static class CustomValidators
    {
        public static bool ValidateSpecialCharacter(string value)
        {
            string strRegex = @"^[a-zA-Z’\-'()/.,\s]+$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(value))
                return (true);
            else
                return (false);
        }
        public static bool ValidateCoordinate(Coordinate coordinate)
        {
            if(coordinate.Row<1 || coordinate.Row>10 || coordinate.Column<1 || coordinate.Column > 10)
            {
                return false;
            }
            return true;
        }
    }
}
