using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipSimple
{
    internal class Ship
    {
        public readonly int Length;
        public readonly char Letter;
        public readonly List<Tuple<int, int>> ShipCoordinates;

        public Ship(int length, char letter, List<Tuple<int, int>> shipCoordinates)
        {
            Length = length;
            Letter = letter;
            ShipCoordinates = shipCoordinates;
        }
    }
}
